using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rdUIWorldSpaceCanvas : MonoBehaviour {
    public Canvas Canvas;
    public string ID;
    // Start is called before the first frame update
    void Start() {
        Canvas = this.GetComponent<Canvas>();
        if (Canvas.renderMode == RenderMode.WorldSpace) {
            Canvas.worldCamera = Camera.main;
        }

        rdUIManager.Seele.OnDestroyPopup += OnDestroyPopup;
    }

    private void OnDestroyPopup(string id) {
        if (id == ID)
            Destroy(this.gameObject);
    }

    private void OnDestroy() {
        if (rdUIManager.Seele != null)
            rdUIManager.Seele.OnDestroyPopup -= OnDestroyPopup;
    }
}
