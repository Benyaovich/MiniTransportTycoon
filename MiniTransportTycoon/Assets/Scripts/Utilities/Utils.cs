using UnityEngine;
using UnityEngine.InputSystem;
using UniVector3 = UnityEngine.Vector3;
using SysVector3 = System.Numerics.Vector3;

public static class Utils
{
    private const int sortingOrderDefault = 5000;
    
    public static UniVector3 UVXZ3(this SysVector3 v3) => new UniVector3(v3.X,0 , v3.Y);
    public static SysVector3 SV3( this UniVector3 v3) => new SysVector3(v3.x,v3.z,v3.y);
    public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = sortingOrderDefault) {
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }
        
    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder) {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }
    
    public static Vector3 GetMouseWorldPosition() {
        Ray ray = Camera.main!.ScreenPointToRay(Mouse.current.position.value);
        if (Physics.Raycast(ray, out RaycastHit raycastHit)) {
            return raycastHit.point;
        } else {
            return Vector3.zero;
        }
    }
}
