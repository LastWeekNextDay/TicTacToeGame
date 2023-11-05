using NUnit.Framework;
using UnityEngine;

public class Edit
{
    [Test]
    public void HasXWon()
    {
        GameObject holder = new GameObject();
        holder.AddComponent<AssetHolder>();
        holder.GetComponent<AssetHolder>().XObjPrefab = Resources.Load<GameObject>("XObj");
        holder.GetComponent<AssetHolder>().OObjPrefab = Resources.Load<GameObject>("OObj");
        holder.GetComponent<AssetHolder>().SlotObjPrefab = Resources.Load<GameObject>("SlotObj");

        GameObject player = new GameObject();
        player.AddComponent<Player>();
        player.GetComponent<Player>().Piece = "X";

        GameObject gameLogic = new GameObject();
        gameLogic.AddComponent<GameLogic>();
        gameLogic.GetComponent<GameLogic>().Player1 = player.GetComponent<Player>();

        TicTacToeGrid tic = new TicTacToeGrid(holder.GetComponent<AssetHolder>(), gameLogic.GetComponent<GameLogic>());
        VictoryCalculator victoryCalculator = new VictoryCalculator(tic,3);
        tic.SetupGrid(3);
        tic.Get(0, 0).IsOccupied = true;
        Assert.IsTrue(gameLogic.GetComponent<GameLogic>().GameActive == false);
    }
}
