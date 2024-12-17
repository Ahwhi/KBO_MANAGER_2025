using GameData;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DropHandler : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        string currentScene = SceneManager.GetActiveScene().name;
        GameObject draggedObject = eventData.pointerDrag;

        DragHandler dragHandler = draggedObject.GetComponent<DragHandler>();
        if (dragHandler == null)
        {
            Debug.LogWarning("�巡�׵� ��ü�� DragHandler�� �������� �ʽ��ϴ�.");
            return;
        }

        if (currentScene == "ManageBatter")
        {
            Batter draggedBatter = draggedObject.GetComponent<DragHandler>().batterInfo;
            Batter currentBatter = this.GetComponent<DragHandler>().batterInfo;
            int draggedNum = draggedBatter.posInTeam;
            int currentNum = currentBatter.posInTeam;
            draggedObject.GetComponent<DragHandler>().batterInfo.posInTeam = currentNum;
            this.GetComponent<DragHandler>().batterInfo.posInTeam = draggedNum;
            ManageBatter.isUpdate = true;
        } else if (currentScene == "ManagePitcher")
        {
            Pitcher draggedPitcher = draggedObject.GetComponent<DragHandler>().pitcherInfo;
            Pitcher currentPitcher = this.GetComponent<DragHandler>().pitcherInfo;
            int draggedNum = draggedPitcher.posInTeam;
            int currentNum = currentPitcher.posInTeam;
            draggedObject.GetComponent<DragHandler>().pitcherInfo.posInTeam = currentNum;
            this.GetComponent<DragHandler>().pitcherInfo.posInTeam = draggedNum;
            ManagePitcher.isUpdate = true;
        }
    }
}