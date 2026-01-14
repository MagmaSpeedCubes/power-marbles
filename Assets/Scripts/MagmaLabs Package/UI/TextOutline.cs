using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace MagmaLabs.UI{
    [AddComponentMenu("UI/Effects/Text Outline")]
    public class TextOutline : BaseMeshEffect
    {

        public Color m_outlineColor = Color.white;
        public float m_outlineThickness = 1f;
        
        public override void ModifyMesh(VertexHelper vh)
        {
            if (enabled)
            {
                Rect rect = graphic.rectTransform.rect;
                int initialVertCount = vh.currentVertCount;

                UIVertex vertex = default(UIVertex);
                for (int i = 0; i < vh.currentVertCount; i++)
                {
                    vh.PopulateUIVertex(ref vertex, i);

                    Vector3 originalPosition = vertex.position;

                    // Offsets for outline effect
                    Vector3[] offsets = new Vector3[]
                    {
                        new Vector3(-m_outlineThickness, -m_outlineThickness, 0),
                        new Vector3(-m_outlineThickness, m_outlineThickness, 0),
                        new Vector3(m_outlineThickness, -m_outlineThickness, 0),
                        new Vector3(m_outlineThickness, m_outlineThickness, 0)
                    };

                    foreach (var offset in offsets)
                    {
                        vertex.position = originalPosition + offset;
                        vertex.color = m_outlineColor;
                        vh.AddVert(vertex);
                    }
                }

                // Rebuild triangles
                int currentVertCount = vh.currentVertCount;
                for (int i = 0; i < initialVertCount; i++)
                {
                    int baseIndex = i * 5; // Original + 4 outline verts
                    for (int j = 0; j < 4; j++)
                    {
                        vh.AddTriangle(baseIndex + j + 1, baseIndex + ((j + 1) % 4) + 1, baseIndex);
                    }
                }



                
            }
        }

    }
}