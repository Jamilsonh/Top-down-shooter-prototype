using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CriticalEffectText : MonoBehaviour
{
    public float floatSpeed = 2f; // Velocidade que o texto vai subir
    public float fadeSpeed = 1f; // Velocidade que o texto vai desaparecer
    public Color textColor = Color.red; // Cor do texto
    private TextMeshProUGUI textMesh; // Referência ao componente TextMeshProUGUI

    private void Start() {
        // Acessa o TextMeshProUGUI dentro do Canvas como um dos filhos
        textMesh = GetComponentInChildren<TextMeshProUGUI>();

        if (textMesh != null) {
            textMesh.color = textColor; // Configura a cor inicial do texto
        }
        else {
            Debug.LogError("TextMeshProUGUI component not found on the object or its children. Please check if the TextMeshProUGUI component is attached.");
        }
    }

    private void Update() {
        // Verifica se o componente textMesh não é nulo antes de tentar acessar suas propriedades
        if (textMesh != null) {
            // Move o texto para cima
            transform.Translate(Vector3.up * floatSpeed * Time.deltaTime);

            // Diminui a opacidade do texto
            Color color = textMesh.color;
            color.a -= fadeSpeed * Time.deltaTime;
            textMesh.color = color;

            // Destrói o texto quando ele estiver completamente transparente
            if (textMesh.color.a <= 0) {
                Destroy(gameObject);
            }
        }
    }
}
