using _Scripts.Managers;

public class DeathView : View
{
    private PlayerEvents _playerEvents;
    
    private void Start()
    {
        Managers.PlayerManager.death += LaunchDeathView;
    }

    private void LaunchDeathView()
    {
        Managers.CamerasManager.ToggleMainCameraMovement(false);
        Show();
    }

    public override void Hide()
    {
        base.Hide();
        
        Managers.CamerasManager.ToggleMainCameraMovement(true);
    }

    private void OnDestroy()
    {
        Managers.PlayerManager.death -= LaunchDeathView;
    }
}
