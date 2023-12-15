using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Windows : MonoBehaviour
{
    [SerializeField] private string[] command;

    private InputField cmd;

    private void Start()
    {
        cmd = GetComponent<InputField>();

        //cmd.onValueChanged.AddListener(delegate { RemoveSpaces(); });
    }

    public void Enter()
    {
        //Не меняйте пожалуйста последовательность

        if (cmd.text == command[0])
        {
            cmd.text = "команда, которую зачастую используют для примера вывода кода на большинстве языках программирования";
        }
        else if (cmd.text == command[1])
        {
            cmd.text = "метод, используемый в этой игре для ходьбы";
        }
        else if (cmd.text == command[2])
        {
            cmd.text = "метод, встроенный в язык программирования C#, для вывода в консоль информацию";
        }
        else if (cmd.text == command[3])
        {
            cmd.text = "метод, встроенный в Unity, который вызывается при столкновенни Collider с Rigidbody на Collider, который является Trigger";
        }
        else if (cmd.text == command[4])
        {
            cmd.text = "например, таким кодом, выполняется кэширование компонента InputField";
        }
        else
        {
            cmd.text = "notSuccess";
        }
    }

    private void RemoveSpaces()
    {
        cmd.text = cmd.text.Replace(" ", "");
    }
}