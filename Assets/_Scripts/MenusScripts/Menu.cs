using _Scripts.Managers;
using UnityEngine;

public abstract class Menu : MonoBehaviour
{
    protected virtual void Start()
    {
        InitUIElements();
    }

    public virtual void Open()
    {
        if (Managers.UIManager.CurrentMenu != null)
        {
            Managers.UIManager.CurrentMenu.Close();
        }
        
        gameObject.SetActive(true);
        Managers.UIManager.SetCurrentMenu(this);
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
    protected abstract void InitUIElements();

    protected void OnBackButtonClicked()
    {
        Managers.UIManager.OpenPreviousMenu();
    }
}
