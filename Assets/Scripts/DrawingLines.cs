using System.Collections.Generic;
using UnityEngine;

public class DrawingLines : MonoBehaviour
{
    [SerializeField]
    private Material _defaultMaterial;

    [SerializeField]
    private GameObject _cursor;

    private MeshRenderer _cursorMesh;

    private LineRenderer _lineRenderer;

    private int _size;
    private Vector2 _currentPosition;
    private Color _currentColor;
    private float _maxAllowableError = 0.15f;

    private GameObject _newGO;

    private List<LineRenderer> _lines;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _size = 1;
        _currentColor = _lineRenderer.startColor;
        _cursorMesh = _cursor.GetComponent<MeshRenderer>();

        _cursorMesh.material.color = _currentColor;
        _cursor.SetActive(true);
    }

    private void Start()
    {
        _lines = new List<LineRenderer>();
        _lines.Add(_lineRenderer);
    }

    private void Update()
    {
        CursorController();

        if (Input.GetMouseButton(0))
        {
            Draw(_lines[_lines.Count - 1]);
        }

        if (Input.GetMouseButtonUp(0))
        {
            CreateNewGameObjectForDrawingLine();
        }
    }

    private void Draw(LineRenderer currentLine)
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitInfo))
        {
            if (Vector2.Distance(hitInfo.point, _currentPosition) > _maxAllowableError)
            {
                currentLine.startColor = _currentColor;
                currentLine.endColor = _currentColor;

                currentLine.positionCount = _size;
                _currentPosition = hitInfo.point;
                currentLine.SetPosition(_size - 1, _currentPosition);

                _size++;
            }
        }
    }

    private void CreateNewGameObjectForDrawingLine()
    {
        _newGO = new GameObject();
        _newGO.transform.SetParent(transform);

        _newGO.AddComponent<LineRenderer>();
        _lineRenderer = _newGO.GetComponent<LineRenderer>();

        _lineRenderer.widthMultiplier = 0.15f;
        _lineRenderer.positionCount = 0;

        _lineRenderer.material = _defaultMaterial;

        _lines.Add(_lineRenderer);
        _size = 1;
    }

    public void RedColor()
    {
        _currentColor = Color.red;
        _cursorMesh.material.color = _currentColor;
    }
    public void GreenColor()
    {
        _currentColor = Color.green;
        _cursorMesh.material.color = _currentColor;
    }
    public void BlueColor()
    {
        _currentColor = Color.blue;
        _cursorMesh.material.color = _currentColor;
    }
    public void WhiteColor()
    {
        _currentColor = Color.white;
        _cursorMesh.material.color = _currentColor;
    }
    public void BrownColor()
    {
        _currentColor = Color.HSVToRGB(30 / 360, 0.75f, 0.4f);
        _cursorMesh.material.color = _currentColor;
    }
    public void ClearTheBoard()
    {
        for (int i = 1; i < _lines.Count; i++)
        {
            Destroy(_lines[i].gameObject);
        }
        _lines[0].positionCount = 0;
        _lines.Clear();
        var firstChild = transform.GetChild(0).gameObject;
        Destroy(firstChild);

        _cursorMesh.material.color = _currentColor;
    }

    private void CursorController()
    {
        var xPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - 0.7f;
        var yPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;

        _cursor.transform.position = new Vector3(xPosition, yPosition, 0);
    }
}
