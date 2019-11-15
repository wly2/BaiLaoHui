using System.Collections.Generic;
using UnityEngine;

public class ThrowItem : MonoBehaviour
{
    private float size;
    public int playerDir;
    public int otherDir;
    public float gameObjectSize;
    public int pengCardPoint = -1;
    public List<GameObject> list;

    public void TestPosition(List<GameObject> game, int _playerDri, int _otherDir)
    {
        list = game;
        playerDir = _playerDri;
        otherDir = _otherDir;
        size = (GameResourceManager.Instance.mjSize3 - GameResourceManager.Instance.mjSize2) / 2;
        switch (_playerDri)
        {
            case 0:
                Own(game, _otherDir);
                break;
            case 1:
                Right(game, _otherDir);
                break;
            case 2:
                Top(game, _otherDir);
                break;
            case 3:
                Left(game, _otherDir);
                break;
        }
    }

    void Own(List<GameObject> game, int otherDir)
    {
        switch (otherDir)
        {
            case 0:
                game[0].transform.localPosition = new Vector3(0, 0, 0);
                game[1].transform.localPosition = new Vector3(-GameResourceManager.Instance.mjSize2, 0, 0);
                game[2].transform.localPosition =
                    new Vector3(game[1].transform.localPosition.x - GameResourceManager.Instance.mjSize2, 0, 0);
                game[3].transform.localPosition =
                    new Vector3(game[2].transform.localPosition.x - GameResourceManager.Instance.mjSize2, 0, 0);
                game[0].transform.eulerAngles = new Vector3(0, 0, 0);
                game[1].transform.eulerAngles = new Vector3(0, 0, 180);
                game[2].transform.eulerAngles = new Vector3(0, 0, 180);
                game[3].transform.eulerAngles = new Vector3(0, 0, 180);
                gameObjectSize = 4 * GameResourceManager.Instance.mjSize2;
                break;
            case 1:
                game[0].transform.localPosition = new Vector3(-size, 0, -size);
                game[0].transform.eulerAngles = new Vector3(0, -90, 0);
                game[1].transform.localPosition =
                    new Vector3(game[0].transform.localPosition.x - GameResourceManager.Instance.mjSize2 - size, 0, 0);
                game[2].transform.localPosition =
                    new Vector3(game[1].transform.localPosition.x - GameResourceManager.Instance.mjSize2, 0, 0);
                if (game.Count == 4)
                {
                    game[3].transform.localPosition =
                        new Vector3(game[2].transform.localPosition.x - GameResourceManager.Instance.mjSize2, 0, 0);
                    gameObjectSize = 3 * GameResourceManager.Instance.mjSize2 + GameResourceManager.Instance.mjSize3;
                }
                else
                {
                    gameObjectSize = 2 * GameResourceManager.Instance.mjSize2 + GameResourceManager.Instance.mjSize3;
                    GameResourceManager.Instance.PlayPengFx(game[0].transform);
                }

                break;
            case 2:
                game[0].transform.localPosition = new Vector3(0, 0, 0);
                game[1].transform.eulerAngles = new Vector3(0, -90, 0);
                game[1].transform.localPosition =
                    new Vector3(game[0].transform.localPosition.x - size - GameResourceManager.Instance.mjSize2, 0,
                        -size);
                game[2].transform.localPosition =
                    new Vector3(game[1].transform.localPosition.x - GameResourceManager.Instance.mjSize2 - size, 0, 0);
                if (game.Count == 4)
                {
                    game[3].transform.localPosition =
                        new Vector3(game[2].transform.localPosition.x - GameResourceManager.Instance.mjSize2, 0, 0);
                    gameObjectSize = 3 * GameResourceManager.Instance.mjSize2 + GameResourceManager.Instance.mjSize3;
                }
                else
                {
                    gameObjectSize = 2 * GameResourceManager.Instance.mjSize2 + GameResourceManager.Instance.mjSize3;
                    GameResourceManager.Instance.PlayPengFx(game[1].transform);
                }

                break;
            case 3:
                game[0].transform.localPosition = new Vector3(0, 0, 0);
                game[1].transform.localPosition = new Vector3(-GameResourceManager.Instance.mjSize2, 0, 0);
                if (game.Count == 4)
                {
                    game[2].transform.localPosition =
                        new Vector3(game[1].transform.localPosition.x - GameResourceManager.Instance.mjSize2, 0, 0);
                    game[2].transform.localEulerAngles = Vector3.zero;
                    game[3].transform.localPosition =
                        new Vector3(game[2].transform.localPosition.x - GameResourceManager.Instance.mjSize2 - size, 0,
                            -size);
                    game[3].transform.eulerAngles = new Vector3(0, -90, 0);
                    gameObjectSize = 3 * GameResourceManager.Instance.mjSize2 + GameResourceManager.Instance.mjSize3;
                }
                else
                {
                    game[2].transform.eulerAngles = new Vector3(0, -90, 0);
                    game[2].transform.localPosition =
                        new Vector3(game[1].transform.localPosition.x - GameResourceManager.Instance.mjSize2 - size, 0,
                            -size);
                    gameObjectSize = 2 * GameResourceManager.Instance.mjSize2 + GameResourceManager.Instance.mjSize3;
                    GameResourceManager.Instance.PlayPengFx(game[2].transform);
                }

                break;
        }
    }

    void Top(List<GameObject> game, int otherDir)
    {
        switch (otherDir)
        {
            case 2:
                game[0].transform.localPosition = new Vector3(0, 0, 0);
                game[1].transform.localPosition = new Vector3(GameResourceManager.Instance.mjSize2, 0, 0);
                game[2].transform.localPosition =
                    new Vector3(game[1].transform.localPosition.x + GameResourceManager.Instance.mjSize2, 0, 0);
                game[0].transform.eulerAngles = new Vector3(180, 0, 0);
                game[1].transform.eulerAngles = new Vector3(180, 0, 0);
                game[2].transform.eulerAngles = new Vector3(180, 0, 0);
                game[3].transform.eulerAngles = new Vector3(180, 0, 0);
                game[3].transform.localPosition =
                    new Vector3(game[2].transform.localPosition.x + GameResourceManager.Instance.mjSize2, 0, 0);
                gameObjectSize = 4 * GameResourceManager.Instance.mjSize2;
                break;
            case 3:
                game[0].transform.localPosition = new Vector3(size, 0, size);
                game[0].transform.eulerAngles = new Vector3(0, 90, 0);
                game[1].transform.eulerAngles = new Vector3(0, 180, 0);
                game[2].transform.eulerAngles = new Vector3(0, 180, 0);
                game[1].transform.localPosition =
                    new Vector3(game[0].transform.localPosition.x + GameResourceManager.Instance.mjSize2 + size, 0, 0);
                game[2].transform.localPosition =
                    new Vector3(game[1].transform.localPosition.x + GameResourceManager.Instance.mjSize2, 0, 0);
                if (game.Count == 4)
                {
                    game[3].transform.localPosition =
                        new Vector3(game[2].transform.localPosition.x + GameResourceManager.Instance.mjSize2, 0, 0);
                    game[3].transform.eulerAngles = new Vector3(0, 180, 0);
                    gameObjectSize = 3 * GameResourceManager.Instance.mjSize2 + GameResourceManager.Instance.mjSize3;

                }
                else
                {
                    gameObjectSize = 2 * GameResourceManager.Instance.mjSize2 + GameResourceManager.Instance.mjSize3;
                    GameResourceManager.Instance.PlayPengFx(game[0].transform);
                }

                break;
            case 0:
                game[0].transform.localPosition = new Vector3(0, 0, 0);
                game[1].transform.eulerAngles = new Vector3(0, 90, 0);
                game[0].transform.eulerAngles = new Vector3(0, 180, 0);
                game[2].transform.eulerAngles = new Vector3(0, 180, 0);
                game[1].transform.localPosition =
                    new Vector3(game[0].transform.localPosition.x + size + GameResourceManager.Instance.mjSize2, 0,
                        size);
                game[2].transform.localPosition =
                    new Vector3(game[1].transform.localPosition.x + GameResourceManager.Instance.mjSize2 + size, 0, 0);
                if (game.Count == 4)
                {
                    game[3].transform.localPosition =
                        new Vector3(game[2].transform.localPosition.x + GameResourceManager.Instance.mjSize2, 0, 0);
                    game[3].transform.eulerAngles = new Vector3(0, 180, 0);
                    gameObjectSize = 3 * GameResourceManager.Instance.mjSize2 + GameResourceManager.Instance.mjSize3;
                }
                else
                {
                    gameObjectSize = 2 * GameResourceManager.Instance.mjSize2 + GameResourceManager.Instance.mjSize3;
                    GameResourceManager.Instance.PlayPengFx(game[1].transform);
                }

                break;
            case 1:
                game[0].transform.localPosition = new Vector3(0, 0, 0);
                game[1].transform.localPosition = new Vector3(GameResourceManager.Instance.mjSize2, 0, 0);

                if (game.Count == 4)
                {
                    game[2].transform.localPosition =
                        new Vector3(game[1].transform.localPosition.x + GameResourceManager.Instance.mjSize2, 0, 0);
                    game[3].transform.localPosition =
                        new Vector3(game[2].transform.localPosition.x + GameResourceManager.Instance.mjSize2 + size, 0,
                            size);
                    game[3].transform.eulerAngles = new Vector3(0, 90, 0);
                    game[1].transform.eulerAngles = new Vector3(0, 180, 0);
                    game[2].transform.eulerAngles = new Vector3(0, 180, 0);
                    game[0].transform.eulerAngles = new Vector3(0, 180, 0);
                    gameObjectSize = 3 * GameResourceManager.Instance.mjSize2 + GameResourceManager.Instance.mjSize3;
                }
                else
                {
                    game[1].transform.eulerAngles = new Vector3(0, 180, 0);
                    game[0].transform.eulerAngles = new Vector3(0, 180, 0);
                    game[2].transform.eulerAngles = new Vector3(0, 90, 0);
                    game[2].transform.localPosition =
                        new Vector3(game[1].transform.localPosition.x + GameResourceManager.Instance.mjSize2 + size, 0,
                            size);
                    gameObjectSize = 2 * GameResourceManager.Instance.mjSize2 + GameResourceManager.Instance.mjSize3;
                    GameResourceManager.Instance.PlayPengFx(game[2].transform);
                }

                break;
        }
    }

    void Right(List<GameObject> game, int otherDir)
    {
        switch (otherDir)
        {
            case 1:
                game[0].transform.localPosition = new Vector3(0, 0, 0);
                game[1].transform.localPosition = new Vector3(0, 0, -GameResourceManager.Instance.mjSize2);
                game[2].transform.localPosition = new Vector3(0, 0,
                    game[1].transform.localPosition.z - GameResourceManager.Instance.mjSize2);
                game[0].transform.eulerAngles = new Vector3(0, -90, 180);
                game[1].transform.eulerAngles = new Vector3(0, -90, 180);
                game[2].transform.eulerAngles = new Vector3(0, -90, 180);
                game[3].transform.eulerAngles = new Vector3(0, -90, 180);
                game[3].transform.localPosition = new Vector3(0, 0,
                    game[2].transform.localPosition.z - GameResourceManager.Instance.mjSize2);
                gameObjectSize = 4 * GameResourceManager.Instance.mjSize2;
                break;
            case 2:
                game[0].transform.localPosition = new Vector3(size, 0, -size);
                game[0].transform.eulerAngles = new Vector3(0, 180, 0);
                game[1].transform.eulerAngles = new Vector3(0, -90, 0);
                game[2].transform.eulerAngles = new Vector3(0, -90, 0);
                game[1].transform.localPosition = new Vector3(0, 0,
                    game[0].transform.localPosition.z - GameResourceManager.Instance.mjSize2 - size);
                game[2].transform.localPosition = new Vector3(0, 0,
                    game[1].transform.localPosition.z - GameResourceManager.Instance.mjSize2);
                if (game.Count == 4)
                {
                    game[3].transform.localPosition = new Vector3(0, 0,
                        game[2].transform.localPosition.z - GameResourceManager.Instance.mjSize2);
                    game[3].transform.eulerAngles = new Vector3(0, -90, 0);
                    gameObjectSize = 3 * GameResourceManager.Instance.mjSize2 + GameResourceManager.Instance.mjSize3;

                }
                else
                {
                    gameObjectSize = 2 * GameResourceManager.Instance.mjSize2 + GameResourceManager.Instance.mjSize3;
                    GameResourceManager.Instance.PlayPengFx(game[0].transform);
                }

                break;
            case 3:
                game[0].transform.localPosition = new Vector3(0, 0, 0);
                game[1].transform.eulerAngles = new Vector3(0, 180, 0);
                game[0].transform.eulerAngles = new Vector3(0, -90, 0);
                game[2].transform.eulerAngles = new Vector3(0, -90, 0);
                game[1].transform.localPosition = new Vector3(size, 0,
                    game[0].transform.localPosition.z - size - GameResourceManager.Instance.mjSize2);
                game[2].transform.localPosition = new Vector3(0, 0,
                    game[1].transform.localPosition.z - GameResourceManager.Instance.mjSize2 - size);
                if (game.Count == 4)
                {
                    game[3].transform.localPosition = new Vector3(0, 0,
                        game[2].transform.localPosition.z - GameResourceManager.Instance.mjSize2);
                    game[3].transform.eulerAngles = new Vector3(0, -90, 0);
                    gameObjectSize = 3 * GameResourceManager.Instance.mjSize2 + GameResourceManager.Instance.mjSize3;
                }
                else
                {
                    gameObjectSize = 2 * GameResourceManager.Instance.mjSize2 + GameResourceManager.Instance.mjSize3;
                    GameResourceManager.Instance.PlayPengFx(game[1].transform);
                }

                break;
            case 0:
                game[0].transform.localPosition = new Vector3(0, 0, 0);
                game[1].transform.localPosition = new Vector3(0, 0, -GameResourceManager.Instance.mjSize2);
                game[0].transform.eulerAngles = new Vector3(0, -90, 0);
                game[1].transform.eulerAngles = new Vector3(0, -90, 0);
                if (game.Count == 4)
                {
                    game[2].transform.localPosition = new Vector3(0, 0,
                        game[1].transform.localPosition.z - GameResourceManager.Instance.mjSize2);
                    game[3].transform.localPosition = new Vector3(size, 0,
                        game[2].transform.localPosition.z - GameResourceManager.Instance.mjSize2 - size);
                    game[2].transform.eulerAngles = new Vector3(0, -90, 0);
                    game[3].transform.eulerAngles = new Vector3(0, 180, 0);
                    gameObjectSize = 3 * GameResourceManager.Instance.mjSize2 + GameResourceManager.Instance.mjSize3;
                }
                else
                {
                    game[2].transform.eulerAngles = new Vector3(0, 180, 0);
                    game[2].transform.localPosition = new Vector3(size, 0,
                        game[1].transform.localPosition.z - GameResourceManager.Instance.mjSize2 - size);
                    gameObjectSize = 2 * GameResourceManager.Instance.mjSize2 + GameResourceManager.Instance.mjSize3;
                    GameResourceManager.Instance.PlayPengFx(game[2].transform);
                }

                break;
        }
    }

    void Left(List<GameObject> game, int otherDir)
    {
        switch (otherDir)
        {
            case 3:
                game[0].transform.localPosition = new Vector3(0, 0, 0);
                game[1].transform.localPosition = new Vector3(0, 0, GameResourceManager.Instance.mjSize2);
                game[2].transform.localPosition = new Vector3(0, 0,
                    game[1].transform.localPosition.z + GameResourceManager.Instance.mjSize2);
                game[0].transform.eulerAngles = new Vector3(0, 90, 180);
                game[1].transform.eulerAngles = new Vector3(0, 90, 180);
                game[2].transform.eulerAngles = new Vector3(0, 90, 180);
                game[3].transform.eulerAngles = new Vector3(0, 90, 180);
                game[3].transform.localPosition = new Vector3(0, 0,
                    game[2].transform.localPosition.z + GameResourceManager.Instance.mjSize2);
                gameObjectSize = 4 * GameResourceManager.Instance.mjSize2;
                break;
            case 0:
                game[0].transform.localPosition = new Vector3(-size, 0, -size);
                game[0].transform.eulerAngles = new Vector3(0, 0, 0);
                game[1].transform.eulerAngles = new Vector3(0, 90, 0);
                game[2].transform.eulerAngles = new Vector3(0, 90, 0);
                game[1].transform.localPosition = new Vector3(0, 0,
                    game[0].transform.localPosition.z + GameResourceManager.Instance.mjSize2 + size);
                game[2].transform.localPosition = new Vector3(0, 0,
                    game[1].transform.localPosition.z + GameResourceManager.Instance.mjSize2);
                if (game.Count == 4)
                {
                    game[3].transform.localPosition = new Vector3(0, 0,
                        game[2].transform.localPosition.z + GameResourceManager.Instance.mjSize2);
                    game[3].transform.eulerAngles = new Vector3(0, 90, 0);
                    gameObjectSize = 3 * GameResourceManager.Instance.mjSize2 + GameResourceManager.Instance.mjSize3;
                }
                else
                {
                    gameObjectSize = 2 * GameResourceManager.Instance.mjSize2 + GameResourceManager.Instance.mjSize3;
                    GameResourceManager.Instance.PlayPengFx(game[0].transform);
                }

                break;
            case 1:
                game[0].transform.localPosition = new Vector3(0, 0, 0);
                game[1].transform.eulerAngles = new Vector3(0, 0, 0);
                game[0].transform.eulerAngles = new Vector3(0, 90, 0);
                game[2].transform.eulerAngles = new Vector3(0, 90, 0);
                game[1].transform.localPosition = new Vector3(-size, 0,
                    game[0].transform.localPosition.z + size + GameResourceManager.Instance.mjSize2);
                game[2].transform.localPosition = new Vector3(0, 0,
                    game[1].transform.localPosition.z + GameResourceManager.Instance.mjSize2 + size);
                if (game.Count == 4)
                {
                    game[3].transform.localPosition = new Vector3(0, 0,
                        game[2].transform.localPosition.z + GameResourceManager.Instance.mjSize2);
                    game[3].transform.eulerAngles = new Vector3(0, 90, 0);
                    gameObjectSize = 3 * GameResourceManager.Instance.mjSize2 + GameResourceManager.Instance.mjSize3;
                }
                else
                {
                    gameObjectSize = 2 * GameResourceManager.Instance.mjSize2 + GameResourceManager.Instance.mjSize3;
                    GameResourceManager.Instance.PlayPengFx(game[1].transform);
                }

                break;
            case 2:
                game[0].transform.localPosition = new Vector3(0, 0, 0);
                game[1].transform.localPosition = new Vector3(0, 0, GameResourceManager.Instance.mjSize2);
                game[0].transform.eulerAngles = new Vector3(0, 90, 0);
                game[1].transform.eulerAngles = new Vector3(0, 90, 0);
                if (game.Count == 4)
                {
                    game[2].transform.localPosition = new Vector3(0, 0,
                        game[1].transform.localPosition.z + GameResourceManager.Instance.mjSize2);
                    game[3].transform.localPosition = new Vector3(-size, 0,
                        game[2].transform.localPosition.z + GameResourceManager.Instance.mjSize2 + size);
                    game[2].transform.eulerAngles = new Vector3(0, 90, 0);
                    game[3].transform.eulerAngles = new Vector3(0, 0, 0);
                    gameObjectSize = 3 * GameResourceManager.Instance.mjSize2 + GameResourceManager.Instance.mjSize3;
                }
                else
                {
                    game[2].transform.eulerAngles = new Vector3(0, 0, 0);
                    game[2].transform.localPosition = new Vector3(-size, 0,
                        game[1].transform.localPosition.z + GameResourceManager.Instance.mjSize2 + size);
                    gameObjectSize = 2 * GameResourceManager.Instance.mjSize2 + GameResourceManager.Instance.mjSize3;
                    GameResourceManager.Instance.PlayPengFx(game[2].transform);
                }

                break;
        }
    }

    public void AddCard()
    {
        GameObject go;
        var obj = GameResourceManager.Instance.CreateGameObjectAndReturn(pengCardPoint, transform, Vector3.zero,
            Vector3.zero);
        go = Instantiate(GameResourceManager.Instance.yinying) as GameObject;
        go.transform.parent = obj.transform;
        go.transform.localPosition = new Vector3(0.13f, -0.28f, 0.01f);
        go.transform.localEulerAngles = new Vector3(90, 90, 90);
        list.Add(obj);
        TestPosition(list, playerDir, otherDir);
    }
}