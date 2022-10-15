using UnityEngine;

public class LevelPiece : MonoBehaviour
{
    [SerializeField][Range(0,20)] int _difficultyLevel = 0;

    public int DifficultyLevel => _difficultyLevel;
}
