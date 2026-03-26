using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FichaStatus : MonoBehaviour
{
    [Header("Textos")]
    public TextMeshProUGUI txtNome, txtNivel, txtAtaque, txtVida;

    [Header("Barra de Experiência")]
    public Slider barraXP;
    public TextMeshProUGUI txtNumerosXP;

    private PlayerEvolution progressoHeroi;
    private CombatAttributes atributosHeroi;

    void Start()
    {
        gameObject.SetActive(false);

        // Localiza a nossa "Fonte de Verdade" na cena
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            progressoHeroi = player.GetComponent<PlayerEvolution>();
            atributosHeroi = player.GetComponent<CombatAttributes>();
        }
    }

    public void AtualizarFicha()
    {
        if (atributosHeroi == null || progressoHeroi == null) return;

        txtNome.text = $"Herói: {atributosHeroi.characterName}";
        txtNivel.text = $"Nível Atual: {atributosHeroi.level}";

        // A Matemática Inversa: O danoAtual do herói JÁ TEM o bônus embutido dentro dele. 
        // Nós apenas subtraímos para criar a exibição visual amigável: Total (Base + Bônus)
        int ataqueBaseSemBonus = atributosHeroi.curDamage - atributosHeroi.bonusAtk;
        int vidaBaseSemBonus = atributosHeroi.currentHP - atributosHeroi.bonusDef;

        txtAtaque.text = $"Ataque: {atributosHeroi.curDamage} (Base {ataqueBaseSemBonus} + {atributosHeroi.bonusAtk})";
        txtVida.text = $"HP Max: {atributosHeroi.currentHP} (Base {vidaBaseSemBonus} + {atributosHeroi.bonusDef})";

        if (barraXP != null)
        {
            int metaDeXP = 0;
            int nivelHeroi = atributosHeroi.level;

            // Busca na tabela de progressão do jogador qual a meta do nível atual
            if (nivelHeroi <= progressoHeroi.nextLevelXP.Length)
                metaDeXP = progressoHeroi.nextLevelXP[nivelHeroi - 1];
            else
                metaDeXP = progressoHeroi.nextLevelXP[progressoHeroi.nextLevelXP.Length - 1];

            barraXP.maxValue = metaDeXP;
            barraXP.value = progressoHeroi.curXP;
            txtNumerosXP.text = $"{progressoHeroi.curXP} / {metaDeXP} XP";
        }
    }
}