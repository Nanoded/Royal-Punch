using UnityEngine;

namespace NewNamespace
{
    public class SelectRandomColor : MonoBehaviour
    {
        [SerializeField] private Color[] _colors;
        [SerializeField] private Material _bodyMaterial;
        [SerializeField] private Material _glovesMaterial;

        private void Awake()
        {
            EventsController.StartEvent.AddListener(() =>
            {
                _bodyMaterial.color = _colors[Random.Range(0, _colors.Length)];
                _glovesMaterial.color = _colors[Random.Range(0, _colors.Length)];
            });
        }
    }
}
