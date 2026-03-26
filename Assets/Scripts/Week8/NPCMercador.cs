using TMPro;
using UnityEngine;

public class NPCMercador : MonoBehaviour
{
    [Header("Interface da Loja")]
    public GameObject painelHub, painelCompra, painelVenda;
    public TextMeshProUGUI textoFeedbackVenda, textoFeedbackCompra;

    [Header("Gerador de Vendas")]
    public Transform containerVendas;
    public GameObject prefabBotaoVenda;

    [Header("Integração Direta com o Jogador")]
    public InventorySystem sistemaInventario;
    public CombatAttributes atributosCombate;
    public DataItem pocaoDeVida;
    public int precoPocao = 50, precoAfiarEspada = 100, precoArmadura = 150;

    private bool jogadorPerto = false;

    void Start()
    {
        // Programação Defensiva: Busca automática caso o desenvolvedor esqueça de arrastar no Inspector!
        if (sistemaInventario == null || atributosCombate == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                sistemaInventario = player.GetComponent<InventorySystem>();
                atributosCombate = player.GetComponent<CombatAttributes>();
            }
        }
    }

    void Update() { if (jogadorPerto && Input.GetKeyDown(KeyCode.E)) AbrirLoja(); }

    private void AbrirLoja()
    {
        painelHub.SetActive(true);
        painelCompra.SetActive(false);
        painelVenda.SetActive(false);
    }
    public void AbrirAbaCompra()
    {
        painelHub.SetActive(false);
        painelCompra.SetActive(true);
    }

    public void FecharTudo()
    {
        painelHub.SetActive(false);
        painelCompra.SetActive(false);
        painelVenda.SetActive(false);
    }

    public void AbrirAbaVenda()
    {
        painelHub.SetActive(false);
        painelVenda.SetActive(true);
        textoFeedbackVenda.text = "O que você encontrou na floresta?";
        GerarListaDeVendas();
    }

    // --- LÓGICA DE COMPRA ---
    public void ComprarPocao()
    {
        if (sistemaInventario.coin >= precoPocao)
        {
            sistemaInventario.ModifyCoin(-precoPocao);
            sistemaInventario.AddItem(pocaoDeVida, 1);
            textoFeedbackCompra.text = "Poção comprada com sucesso!";
        }
        else textoFeedbackCompra.text = "Ouro insuficiente!";
    }

    public void ComprarMelhoriaEspada()
    {
        if (sistemaInventario.coin >= precoAfiarEspada)
        {
            sistemaInventario.ModifyCoin(-precoAfiarEspada);

            // Injeta o bônus direto no herói e obriga o sistema a reconhecer o novo dano na hora!
            atributosCombate.bonusAtk += 10;
            atributosCombate.CalculateStatus();

            precoAfiarEspada += 50;
            textoFeedbackCompra.text = "Espada afiada! (+10 Ataque)";
        }
        else textoFeedbackCompra.text = "Ouro insuficiente!";
    }

    public void ComprarMelhoriaArmadura()
    {
        if (sistemaInventario.coin >= precoArmadura)
        {
            sistemaInventario.ModifyCoin(-precoArmadura);

            // Injeta o bônus de defesa e cura a vida extra concedida
            atributosCombate.bonusDef += 25;
            atributosCombate.CalculateStatus();
            atributosCombate.currentHP += 25;
            atributosCombate.UpdateBar();

            precoArmadura += 75;
            textoFeedbackCompra.text = "Armadura reforçada! (+25 Vida)";
        }
        else textoFeedbackCompra.text = "Ouro insuficiente!";
    }

    // --- LÓGICA DE VENDA DINÂMICA ---
    private void GerarListaDeVendas()
    {
        // Padrão de Reconstrução: Limpa todos os botões antigos antes de ler a mochila
        foreach (Transform filho in containerVendas) Destroy(filho.gameObject);

        // Lê a Memória Global e instancializa apenas o que tem valor comercial
        foreach (InventorySlots slot in sistemaInventario.inventory)
        {
            if (slot.itemData.value >= 2 && slot.quantity > 0)
            {
                GameObject novoBotao = Instantiate(prefabBotaoVenda, containerVendas);
                novoBotao.GetComponent<BotaoVendaUI>().ConfigurarBotao(slot.itemData, slot.quantity, this);
            }
        }
    }

    public void ExecutarVenda(DataItem itemParaVender)
    {
        int lucro = itemParaVender.value / 2;
        sistemaInventario.ModifyCoin(lucro);

        if (sistemaInventario != null) sistemaInventario.RemoveItem(itemParaVender, 1);

        textoFeedbackVenda.text = "Vendeu " + itemParaVender.itemName + " por " + lucro + " Ouro!";

        // Reconstrói a lista visualmente para mostrar a quantidade abatida
        GerarListaDeVendas();
    }

    private void OnTriggerEnter2D(Collider2D col) { if (col.CompareTag("Player")) jogadorPerto = true; }
    private void OnTriggerExit2D(Collider2D col) { if (col.CompareTag("Player")) { jogadorPerto = false; FecharTudo(); } }

}
