public class AutoSaveManager
{
    public bool CanAutoSave
    {
        get { return _canAutoSave; }
        set { _canAutoSave = value; }
    }

    bool _canAutoSave = false;
}
