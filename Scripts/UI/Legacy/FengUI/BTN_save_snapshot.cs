




using ApplicationManagers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BTN_save_snapshot : MonoBehaviour
{
    public GameObject info;
    public GameObject targetTexture;
    public GameObject[] thingsNeedToHide;

    private void Awake()
    {
        this.info.GetComponent<UILabel>().text = string.Empty;
    }

    private void OnClick()
    {
        foreach (GameObject obj2 in this.thingsNeedToHide)
        {
            Transform transform = obj2.transform;
            transform.position += (Vector3) (Vector3.up * 10000f);
        }
        base.StartCoroutine(this.ScreenshotEncode());
        this.info.GetComponent<UILabel>().text = "Saving...";
    }

    [DebuggerHidden]
    private IEnumerator ScreenshotEncode()
    {
        return new ScreenshotEncodecIterator0 { fthis = this };
    }

    [CompilerGenerated]
    private sealed class ScreenshotEncodecIterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object Scurrent;
        internal int SPC;
        internal GameObject[] Ss_52;
        internal int Ss_63;
        internal BTN_save_snapshot fthis;
        internal GameObject go4;
        internal string img_name5;
        internal float r0;
        internal Texture2D texture1;

        [DebuggerHidden]
        public void Dispose()
        {
            this.SPC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.SPC;
            this.SPC = -1;
            switch (num)
            {
                case 0:
                    this.Scurrent = new WaitForEndOfFrame();
                    this.SPC = 1;
                    goto Label_0308;

                case 1:
                    this.r0 = ((float) Screen.height) / 600f;
                    this.texture1 = new Texture2D((int) (this.r0 * this.fthis.targetTexture.transform.localScale.x), (int) (this.r0 * this.fthis.targetTexture.transform.localScale.y), TextureFormat.RGB24, false);
                    this.texture1.ReadPixels(new Rect((Screen.width * 0.5f) - (this.texture1.width * 0.5f), ((Screen.height * 0.5f) - (this.texture1.height * 0.5f)) - (this.r0 * 0f), (float) this.texture1.width, (float) this.texture1.height), 0, 0);
                    this.texture1.Apply();
                    this.Scurrent = 0;
                    this.SPC = 2;
                    goto Label_0308;

                case 2:
                {
                    this.Ss_52 = this.fthis.thingsNeedToHide;
                    this.Ss_63 = 0;
                    while (this.Ss_63 < this.Ss_52.Length)
                    {
                        this.go4 = this.Ss_52[this.Ss_63];
                        Transform transform = this.go4.transform;
                        transform.position -= (Vector3) (Vector3.up * 10000f);
                        this.Ss_63++;
                    }
                    string[] textArray1 = new string[] { DateTime.Today.Month.ToString(), DateTime.Today.Day.ToString(), DateTime.Today.Year.ToString(), "-", DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString(), DateTime.Now.Second.ToString(), ".png" };
                    this.img_name5 = string.Concat(textArray1);
                    object[] args = new object[] { this.img_name5, this.texture1.width, this.texture1.height, Convert.ToBase64String(this.texture1.EncodeToPNG()) };
                    SnapshotManager.SaveSnapshotFinish(texture1, img_name5);
                    UnityEngine.Object.DestroyObject(this.texture1);
                    this.fthis.info.GetComponent<UILabel>().text = string.Format("Saved snapshot to {0}", SnapshotManager.SnapshotPath);
                    this.SPC = -1;
                    break;
                }
            }
            return false;
        Label_0308:
            return true;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.Scurrent;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.Scurrent;
            }
        }
    }
}

