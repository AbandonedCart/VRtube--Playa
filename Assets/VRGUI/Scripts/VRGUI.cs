using UnityEngine;
using System.Collections;

public abstract class VRGUI : MonoBehaviour 
{
	public Vector3 guiPosition      = new Vector3(0f, 0f, 1f);
	public float   guiSize          = 1f;
	public bool    useCurvedSurface = true;
	public bool    acceptMouse      = true;
	public bool    acceptKeyboard   = true;
	public int     cursorSize       = 32;
	public Texture customCursor     = null;
	
	private GameObject    guiRenderPlane    = null;
	private RenderTexture guiRenderTexture  = null;
	private Vector2       cursorPosition    = Vector2.zero;
	private Texture       cursor            = null;
	
	private bool isInitialized = false;
	
	private void Initialize () 
	{
		// create the render plane
		if (useCurvedSurface)
		{
			guiRenderPlane = Instantiate(Resources.Load("VRGUICurvedSurface")) as GameObject;
		}
		else
		{
			guiRenderPlane = Instantiate(Resources.Load("VRGUIFlatSurface")) as GameObject;
		}
		
		// position the render plane
		guiRenderPlane.transform.parent        = this.transform;
		guiRenderPlane.transform.localPosition = guiPosition;
		if (transform.rotation.y == 180f) {
			guiRenderPlane.transform.localRotation = Quaternion.identity;
		} else {
			guiRenderPlane.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
		}
		guiRenderPlane.transform.localScale    = new Vector3(guiSize, guiSize, guiSize);
		
		// create the render texture
		guiRenderTexture = new RenderTexture(Screen.width, Screen.height, 24);
		
		// assign the render texture to the render plane
		guiRenderPlane.GetComponent<Renderer>().material.mainTexture = guiRenderTexture;
		
		if (acceptMouse)
		{
			loadCursorGraphic();
		}
		isInitialized = true;
	}
	
	protected void OnEnable()
	{
		if (guiRenderPlane != null)
		{
			guiRenderPlane.SetActive(true);
		}
	}
	
	protected void OnDisable()
	{
		if (guiRenderPlane != null)
		{
			guiRenderTexture.DiscardContents();
			guiRenderPlane.SetActive(false);
		}
	}

	private void loadCursorGraphic() {
		if (cursor == null) {
			// create the cursor
			if (customCursor != null)
			{
				cursor = customCursor;
			}
			else
			{
				cursor = Resources.Load("Textures/SimpleCursor") as Texture;
			}
		}
	}
	
	protected void OnGUI()
	{
		if (!isInitialized) {
			Initialize();
		}
		
		// handle mouse events
		if (Event.current.isMouse)
		{
			// return if not accepting mouse events
			if (!acceptMouse)
			{
				return;
			}
		}
		if (acceptMouse)
		{
			loadCursorGraphic();
			// save the mouse position
			cursorPosition = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
		}
		
		// handle key events
		if (Event.current.isKey)
		{
			// return if not accepting key events
			if (!acceptKeyboard)
			{
				return;
			}
		}
		
		// save current render texture
		RenderTexture tempRenderTexture = RenderTexture.active; 
		
		// set the render texture to render the GUI onto
		if (Event.current.type == EventType.Repaint)
		{
			RenderTexture.active = guiRenderTexture;
			GL.Clear (true, true, Color.clear, 1.0f);
		}
		
		// draw the VRGUI
		OnVRGUI();
		
		if (Event.current.type == EventType.Repaint)
		{	
			if (acceptMouse)
			{
				// draw the cursor
				GUI.DrawTexture(new Rect(cursorPosition.x, cursorPosition.y, cursorSize, cursorSize), cursor, ScaleMode.StretchToFill);
			}
			
			// restore the previous render texture
			RenderTexture.active = tempRenderTexture;
		}
	}
	
	public abstract void OnVRGUI();
}