public interface IInteractable
{
    void Interact(PlayerContext context);
    void ExitInteract(PlayerContext context); 
    InteractionType InteractionType { get; }
    bool StartsPuzzle { get; } // opcional, se quiser manter
    InteractionType PuzzleType { get; } // usado se for Puzzle
    string GetInteractionPrompt(); // Opcional: para mostrar "Pressione E para interagir"
}
