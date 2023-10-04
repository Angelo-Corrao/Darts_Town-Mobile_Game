using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
 * shows the realtime dart in DartCustomization scene
 */

public class UI3D : MonoBehaviour {

	private RenderTexture rt = null;
	[SerializeField]
	private Camera renderCamera = null;
	[SerializeField]
	private RawImage dartModel = null;

	void Awake() {
		Rect rect = dartModel.rectTransform.rect;
		rt = new RenderTexture((int)rect.width, (int)rect.height, 32);
		renderCamera.targetTexture = rt;
		dartModel.texture = rt;
	}

	void OnDestroy() {
		if (rt != null) {
			dartModel.texture = null;
			rt.Release();
		}
	}
}
