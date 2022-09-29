using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bot : MonoBehaviour
{
    [SerializeField]
    private DragAndDropBuilding builder;

    [Space]

    [Header("спавн зданий ботом")]

    [SerializeField]
    private List<float> timeDelay;

    [SerializeField]
    private List<UnitData> unit;

    [SerializeField]
    private List<Vector2> coords;

    public IEnumerator BotBuilder() {

        int x = 0;

        while (x < unit.Count)
        {
            yield return new WaitForSeconds(timeDelay[x]);

            builder.StartPlacing(unit[x], false); 
 
            builder.PlaceBuild((int)coords[x].x, (int)coords[x].y, 1, 2, Color.cyan, false);

            x++;
        }
    }

    public void StartBot() {
        StartCoroutine(BotBuilder());
    }

    public void StopBot()
    {
        StopCoroutine(BotBuilder());
    }
}
