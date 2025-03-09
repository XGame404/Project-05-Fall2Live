using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    [SerializeField] private float ScrollSpeed = 0.3f;

    private MeshRenderer _meshRenderer;

    void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        Vector2 offset = _meshRenderer.sharedMaterial.GetTextureOffset("_MainTex");
        offset.y += Time.deltaTime * ScrollSpeed;
        _meshRenderer.material.SetTextureOffset("_MainTex", offset);
    }

}
