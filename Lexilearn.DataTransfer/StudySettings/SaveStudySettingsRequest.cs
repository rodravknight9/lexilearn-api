namespace Lexilearn.DataTransfer.StudySettings;

public class SaveStudySettingsRequest
{
    public int SessionSize { get; set; }
    public int NewCardsPercentage { get; set; }
    public int HardCardsPercentage { get; set; }
}
