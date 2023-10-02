using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]private Sprite[] playIcons;
    [SerializeField]private Button[] tictactoeSpaces;

    private int WhoTurn;
    private int turnCount;
    private int[] markedSpaces = new int[9];
    private bool _endGame;

    [SerializeField]private GameObject _gameOverCanvas;
    [SerializeField]private TMP_Text _winner;

    private List<SpriteState> _spriteState;

    private void Start()
    {
        if(_spriteState == null) { InitSpriteState(); }
        LoadGame();
    }

    void LoadGame()
    {
        turnCount = 0;

        for (int i = 0; i < tictactoeSpaces.Length; i++)
        {
            if (!PlayerPrefs.HasKey(i.ToString()))  
                PlayerPrefs.SetInt(i.ToString(), 0);
            int tmpState = PlayerPrefs.GetInt(i.ToString());
            tictactoeSpaces[i].interactable = !Convert.ToBoolean(tmpState);
            tictactoeSpaces[i].GetComponent<Image>().sprite = playIcons[tmpState];
            if (tmpState == 0)
            {
                markedSpaces[i] = 100;
            }
            else
            {
                markedSpaces[i] = tmpState;
            }
            if(tmpState != 0)
            {
                turnCount++;
            }
        }
        WhoTurn = turnCount % 2;
        for (int i = 0; i < tictactoeSpaces.Length; i++)
        {
            tictactoeSpaces[i].GetComponent<Button>().spriteState = _spriteState[WhoTurn];
        }
        GameEnd();
    }

    private void InitSpriteState()
    {
        _spriteState = new List<SpriteState>();
        SpriteState tmpSpriteState = new SpriteState();
        tmpSpriteState.highlightedSprite = playIcons[1];
        tmpSpriteState.pressedSprite = playIcons[1];
        _spriteState.Add(tmpSpriteState);
        tmpSpriteState.highlightedSprite = playIcons[2];
        tmpSpriteState.pressedSprite = playIcons[2];
        _spriteState.Add(tmpSpriteState);
    }

    private void ChangeSpriteState()
    {
        for (int i = 0; i < tictactoeSpaces.Length; i++)
        {
            tictactoeSpaces[i].GetComponent<Button>().spriteState = _spriteState[WhoTurn];
        }
    }

    public void TicTacToeButton(int pos)
    {
        tictactoeSpaces[pos].image.sprite = playIcons[WhoTurn+1];
        tictactoeSpaces[pos].interactable = false;
        markedSpaces[pos] = WhoTurn + 1;
        PlayerPrefs.SetInt(pos.ToString(), WhoTurn + 1);
        turnCount++;

        if (turnCount > 4)
        {
            GameEnd();
        }

        WhoTurn = (WhoTurn == 1) ? 0 : 1;

        ChangeSpriteState();
    }

    public void Restart()
    {
        GameManager.Instance.RestartGame();
    }

    void WinnerCheck()
    {
        int s1 = markedSpaces[0] + markedSpaces[1] + markedSpaces[2];
        int s2 = markedSpaces[3] + markedSpaces[4] + markedSpaces[5];
        int s3 = markedSpaces[6] + markedSpaces[7] + markedSpaces[8];
        int s4 = markedSpaces[0] + markedSpaces[3] + markedSpaces[6];
        int s5 = markedSpaces[1] + markedSpaces[4] + markedSpaces[7];
        int s6 = markedSpaces[2] + markedSpaces[5] + markedSpaces[8];
        int s7 = markedSpaces[0] + markedSpaces[4] + markedSpaces[8];
        int s8 = markedSpaces[2] + markedSpaces[4] + markedSpaces[6];
        var solutions = new int[] { s1, s2, s3, s4, s5, s6, s7, s8 };
        for (int i = 0; i < solutions.Length; i++)
        {
            if(solutions[i] == 3) 
            {
                _endGame = true;
                _winner.text = "Cross win";
            }
            if (solutions[i] == 6) {
                _endGame = true;
                _winner.text = "Nought win";
            }
        }
    }

    void GameEnd()
    {
        if (turnCount == 9)
        {
            _endGame = true;
            _winner.text = "Noone win";
        }
        WinnerCheck();
        _gameOverCanvas.SetActive(_endGame);
    }
}
