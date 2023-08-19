public class AutoSaveManager
{
    private bool _canAutoSave = false;

    public bool CanAutoSave
    {
        get { return _canAutoSave; }
        set { _canAutoSave = value; }
    }
}
