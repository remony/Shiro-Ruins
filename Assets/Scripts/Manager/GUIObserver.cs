using System;


public interface GuiObserver
{
    void displayLastMessage();
    void MessageUpdate(string message);
    void AddScore(int score);
    void UpdateHealth(int health);
    void ChangeState(int id);
    void updateMagicCooldown(float count);
    void changeGrounded(bool grounded);
    void changeInput(int type);
}
