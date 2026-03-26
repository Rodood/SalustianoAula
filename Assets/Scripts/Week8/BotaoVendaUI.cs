using UnityEngine;
using TMPro;

public class BotaoVendaUI : MonoBehaviour
{
    public TextMeshProUGUI textoBotao;
    private DataItem itemDesteBotao;
    private NPCMercador mercadorVinculado;

    public void ConfigurarBotao(DataItem item, int quantidade, NPCMercador mercador)
    {
        itemDesteBotao = item;
        mercadorVinculado = mercador;

        int precoDeVendaAoMercador = item.value / 2;
        textoBotao.text = $"{item.itemName} (x{quantidade}) - {precoDeVendaAoMercador} Ouro";
    }

    // Lembre-se de linkar esta função no evento OnClick() do Prefab!
    public void ClicouVender() { mercadorVinculado.ExecutarVenda(itemDesteBotao); }
}