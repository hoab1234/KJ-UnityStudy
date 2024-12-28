using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System;
using TMPro;

[Serializable]
public class PokemonData
{
    public int id;
    public int height;
    public int base_experience;
    public Ability[] abilities;
    public Sprites sprites;
}

[Serializable]
public class Ability
{
    public AbilityDetail ability;
    public bool is_hidden;
}

[Serializable]
public class AbilityDetail
{
    public string name;
}

[Serializable]
public class Sprites
{
    public string front_default;
}

public class PokemonDisplay : MonoBehaviour
{

    [SerializeField] private Image pokemonImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text idText;
    [SerializeField] private TMP_Text heightText;
    [SerializeField] private TMP_Text baseExpText;
    [SerializeField] private TMP_Text abilitiesText;

    private void Start()
    {
        StartCoroutine(GetPokemonData());
    }

    IEnumerator GetPokemonData()
    {
        // ��ī�� ������ ��������

        // API ��������Ʈ URL�� �����մϴ�
        string url = "https://pokeapi.co/api/v2/pokemon/pikachu";

        // UnityWebRequest�� ����Ͽ� HTTP GET ��û�� �����մϴ�.
        // UnityWebRequest: Unity���� �� ��û�� ó���ϱ� ���� Ŭ����
        // Get(): HTTP GET ��û�� �����ϴ� �޼���
        // SendWebRequest(): ���� �� ��û�� ������ �޼���
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest(); // �� ��û�� ������ ������ ��ٸ��ϴ�.

            // ��û�� ���������� �Ϸ�Ǿ����� Ȯ��
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("��ī�� ������: " + request.downloadHandler.text);
                // JSON �Ľ� �� ������ Ȱ��

                  // JSON�� PokemonData ��ü�� ��ȯ
                PokemonData pokemonData = JsonUtility.FromJson<PokemonData>(request.downloadHandler.text);
                
                // UI ������Ʈ
                UpdatePokemonInfo(pokemonData);
                
                // �̹��� �ٿ�ε�
                if (!string.IsNullOrEmpty(pokemonData.sprites.front_default))
                {
                    StartCoroutine(GetPokemonSprite(pokemonData.sprites.front_default));
                }
            }
            else
            {
                Debug.LogError("����: " + request.error);
            }
        } // using ����� ������ request ��ü�� �ڵ����� �����˴ϴ�.
    }

    void UpdatePokemonInfo(PokemonData data)
    {
        // �⺻ ���� ������Ʈ
        nameText.text = "Name: Pikachu";
        idText.text = "ID: " + data.id;
        heightText.text = "Height: " + data.height * 10 + "cm";  // API������ ���ù��� ������ ����
        baseExpText.text = "Base Experience: " + data.base_experience;

        // Ư�� ���� ������Ʈ
        string abilities = "Abilities:\n";
        foreach (var ability in data.abilities)
        {
            abilities += $"- {ability.ability.name} {(ability.is_hidden ? "(Hidden)" : "")}\n";
        }
        abilitiesText.text = abilities;
    }

    IEnumerator GetPokemonSprite(string spriteUrl)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(spriteUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                pokemonImage.sprite = Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f)
                );
            }
            else
            {
                Debug.LogError("�̹��� �ٿ�ε� ����: " + request.error);
            }
        }
    }
}
