using System;
using System.IO;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Utility;

namespace ApplicationManagers
{
    public class AutoUpdateManager : MonoBehaviour
    {
        public static AutoUpdateStatus Status = AutoUpdateStatus.Updating;
        public static bool CloseFailureBox = false;
        static AutoUpdateManager _instance;

        // consts
        static readonly string RootDataPath = Application.dataPath;
        static readonly string Platform = File.ReadAllText(RootDataPath + "/PlatformInfo");
        static readonly string RootUpdateURL = "http://aottgrc.com/Patch";
        static readonly string LauncherVersionURL = RootUpdateURL + "/LauncherVersion.txt";
        public static readonly string PlatformUpdateURL = RootUpdateURL + "/" + Platform;
        static readonly string ChecksumURL = PlatformUpdateURL + "/Checksum.txt";

        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
            StartUpdate();
        }

        public static void StartUpdate()
        {
            if (ApplicationConfig.DevelopmentMode)
                Status = AutoUpdateStatus.Updated;
            else
                _instance.StartCoroutine(_instance.StartUpdateCoroutine());
        }

        IEnumerator StartUpdateCoroutine()
        {
            Status = AutoUpdateStatus.Updating;
            List<string> fileChecksums;
            bool downloadedFile = false;

            // check that mac is in Applications folder to avoid translocation
            if (Application.platform == RuntimePlatform.OSXPlayer && !RootDataPath.Contains("Applications"))
            {
                Status = AutoUpdateStatus.MacTranslocated;
                yield break;
            }

            // check launcher version
            using (WWW www = new WWW(LauncherVersionURL))
            {
                yield return www;
                if (www.error != null)
                {
                    OnUpdateFail("Error fetching launcher version", www.error);
                    yield break;
                }
                float f;
                if (!float.TryParse(www.text, out f))
                {
                    OnUpdateFail("Received an invalid launcher version", www.text);
                    yield break;
                }
                if (www.text != ApplicationConfig.LauncherVersion)
                {
                    OnLauncherOutdated();
                    yield break;
                }
            }

            // fetch latest checksum (list of file paths and their checksums)
            using (WWW www = new WWW(ChecksumURL))
            {
                yield return www;
                if (www.error != null)
                {
                    OnUpdateFail("Error fetching checksum", www.error);
                    yield break;
                }
                fileChecksums = www.text.Split('\n').ToList();
            }

            // compare local file checksums to server checksums, and download file if diff is found
            foreach (string fileChecksum in fileChecksums)
            {
                string[] strArray = fileChecksum.Split(':');
                string fileName = strArray[0].Trim();
                string checksum = strArray[1].Trim();
                string localChecksum;
                string filePath = RootDataPath + "/" + fileName;
                if (File.Exists(filePath))
                {
                    try
                    {
                        localChecksum = GenerateMD5(filePath);
                    }
                    catch (Exception ex)
                    {
                        OnUpdateFail("Error generating checksum for " + fileName, ex.Message);
                        yield break;
                    }
                }
                else
                    localChecksum = string.Empty;
                if (localChecksum != checksum)
                {
                    Debug.Log("File diff found, downloading " + fileName);
                    downloadedFile = true;
                    using (WWW www = new WWW(PlatformUpdateURL + "/" + fileName))
                    {
                        yield return www;
                        if (www.error != null)
                        {
                            OnUpdateFail("Error fetching file " + fileName, www.error);
                            yield break;
                        }
                        try
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                            File.WriteAllBytes(filePath, www.bytes);
                        }
                        catch (Exception ex)
                        {
                            OnUpdateFail("Error writing file " + fileName, ex.Message);
                            yield break;
                        }
                    }
                }
            }
            if (downloadedFile)
                Status = AutoUpdateStatus.NeedRestart;
            else
                Status = AutoUpdateStatus.Updated;
        }

        private void OnUpdateFail(string message, string error)
        {
            Debug.Log(message + ": " + error);
            Status = AutoUpdateStatus.FailedUpdate;
        }

        private void OnLauncherOutdated()
        {
            Status = AutoUpdateStatus.LauncherOutdated;
        }

        private string GenerateMD5(string filePath)
        {
            byte[] fileBytes = File.ReadAllBytes(filePath);
            StringBuilder sb = new StringBuilder();
            MD5 md5 = MD5.Create();
            byte[] hashBytes = md5.ComputeHash(fileBytes);
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }

    public enum AutoUpdateStatus
    {
        Updated,
        Updating,
        NeedRestart,
        FailedUpdate,
        LauncherOutdated,
        MacTranslocated
    }
}