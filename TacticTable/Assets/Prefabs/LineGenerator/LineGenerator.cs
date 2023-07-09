using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class LineGenerator : MonoBehaviour
{
    [SerializeField] private LineRenderer line;                     // pelda vonal
    [SerializeField] public float lineThickness = 0.1f;             // vonal vastagsaga
    [SerializeField] public float minDistanceBetweenPoints = .1f;   // minimum tavolsag 2 pont kozott, nagyobb erteknel szogletesebb a vonal
    
    public Color lineColor;                 // a vonal szine, ezt kell majd valtoztatni, primitiv pelda: GetComponent<LineGenerator>().lineColor = Color.red;

    private RectTransform rect;
    private LineRenderer currentLine;       // az adott vonal amit rajzolunk / huzunk
    private int lineCount = 0;              // eddig hany vonal volt letrehozva avagy a kovetkezo vonal indexe
    private bool mouseDown = false;         // a bal egergomb eppen le van-e nyomva
    private bool mouseInside = false;

    // vonalak listaja kesobbi fejlesztesekhez
    private List<LineRenderer> lines;

    private void Awake()
    {
        lines = new List<LineRenderer>();
        lineColor = Color.white;
        rect = GetComponent<RectTransform>();
    }

    // Minden frame-en (kepfrissitesen) megnezi hogy a bal eger gomb le van-e nyomva es az alapjan rajzol vagy nem
    void Update()
    {
        CheckMouseInput();

        CheckMousePos();

        CheckIfNeedToDraw();

        CheckRemoveLines();
    }

    private void CheckMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Amikor lenyomjuk az egeret letrehoz egy uj vonalat
            CreateNewLinesegment();
            mouseDown = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            mouseDown = false;
        }
    }

    private void CheckMousePos()
        {
            float left = rect.rect.xMin + rect.position.x;
            float right = rect.rect.xMax + rect.position.x;

            float bottom = rect.rect.yMin + rect.position.y;
            float top = rect.rect.yMax + rect.position.y;

            Vector2 mouse = MouseWorldPos();

            if (left < mouse.x && right > mouse.x &&
                bottom < mouse.y && top > mouse.y)
            {
                if (mouseDown && !mouseInside)
                {
                    CreateNewLinesegment();
                }
                mouseInside = true;
            }
            else
            {
                mouseInside = false;
            }
        }

    private void CheckIfNeedToDraw()
    {
        if (mouseDown && mouseInside)
        {
            Draw();
        }
    }

    private void CheckRemoveLines()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            RemoveAllLines();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            RemoveLastLine();
        }
    }

    // Letrehoz egy uj vonalat amit a tovabbiakban az eger felemeleseig bovitunk pontokkal
    private void CreateNewLinesegment()
    {
            // Lemasolja a pelda vonalat amit a szerkeszto fullben megadtunk majd elmenti egy listaba (ezzel akar majd lehet torolni vonalakat)
        currentLine = Instantiate(line);
        lines.Add(currentLine);

            // A sorting orderrel lehet beallitani hogy felulre rajzolja a vonalat, minnel nagyobb annal feljebb kerul a vonal
            // Itt a "lineCount" szamlalo segitsegevel mindig a legujjabb vonal van legfelul
        currentLine.sortingOrder = lineCount;

            // A vonal vastagsaga egy AnimationCurve-ben van elmentve ezert letrehoz egyet es egy erteket ad meg neki, igy ugyanolyan vastag a vonal mindenhol
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0, lineThickness);
        currentLine.widthCurve = curve;

            // Itt berakja a vonalat a "LineGenerator" gameObject ala (mint egy mappa) hogy ne legyen szanaszet majd ad neki nevet a konnyebb Debug kedveert
        currentLine.transform.SetParent(transform);
        currentLine.name = string.Format("Line ({0})", transform.childCount);

            // A "SetLineMaterial" fuggvenybol visszakapott anyagot beallitja a vonal anyaganak
        currentLine.material = SetLineMaterial();

        lineCount++;
    }

    // Lemasolja a vonal anyagat az "Instantiate()" fuggvenyel es az ujjat modositja
    // Igy van megoldva hogy kulonbozo szinuek lehessenek a vonalak
    private Material SetLineMaterial()
    {
        Material material = Instantiate(currentLine.material);
        material.color = lineColor;

        material.EnableKeyword("_EMISSION");
        material.SetColor("_EmissionColor", lineColor);

        return material;
    }

    // Meghivaj a mostani vonal pont lerakos fuggvenyet
    private void Draw()
    {
        currentLine.GetComponent<LineScript>().AddPoint(MouseWorldPos(), minDistanceBetweenPoints);
    }

    // Visszaadja az eger mostani poziciojat a jatekteren belul
    private Vector3 MouseWorldPos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition) + (Vector3.forward * Camera.main.transform.position.z * -1);
    }

    public void RemoveAllLines()
    {
        foreach (LineRenderer line in lines)
        {
            Destroy(line.gameObject);
        }
        lineCount = 0;
        lines.Clear();
    }

    public void RemoveLastLine()
    {
        if(lineCount > 0)
        {
            Destroy(lines.Last().gameObject);
            lineCount -= 1;
            lines.RemoveAt(lineCount);
        }
    }

    public void RemoveLastNLine(int count)
    {
        for (int i = 0; i < count; i++)
        {
            RemoveLastLine();
        }
    }
}
