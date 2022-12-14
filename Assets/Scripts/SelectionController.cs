using System.Collections.Generic;
using System.Linq;
using Rewired;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles selecting tiles on a per-player basis - there should be one instance of this per player.
///
/// The origin is at the top left corner, so X increases as selection moves to the right and Y increases as selection moves down. 
/// </summary>
public class SelectionController : MonoBehaviour {
    public List<HorizontalLayoutGroup> Rows;
    public PlayerManager.PlayerID ID;
    public SoundCall MoveSound;

    public float ReselectionCooldown;

    public int XPos;
    public int YPos;

    private const string ConfirmButton = "Confirm";
    private const string HorizontalMovement = "Move Horizontal";
    private const string VerticalMovement = "Move Vertical";
    private const float MinInputAmount = .15f;

    private Rewired.Player _player;
    private List<List<GameButton>> _buttonRows = new List<List<GameButton>>();
    private float _reselectTimer;
    private Vector2 _lastInputVector;

    public delegate void SelectionChanged();
    public event SelectionChanged OnAbilityUpdated;

    public GameButton SelectedButton => _buttonRows[YPos][XPos];
    
    // Start is called before the first frame update
    private void Start() {
        _player = ReInput.players.GetPlayer((int)ID);

        SetupButtons();

        ResetSelection();
    }
    
    private void Update() {
        if (GameManager.Instance.GameOverTriggered) return;
        UpdateNavigation();
        if (_player.GetButtonDown("Confirm")) {
            PressSelectedButton();
        }

    }

    private void SetupButtons() {
        foreach (HorizontalLayoutGroup h in Rows) {
            _buttonRows.Add(h.GetComponentsInChildren<GameButton>().ToList());
        }
    }

    private void PlayButtonMoveSound() {
        SoundManager.thisSoundManager.PlaySound(MoveSound, gameObject);
    }
    
    #region Navigation

    private void UpdateNavigation() {
        _reselectTimer += Time.deltaTime;
        float horizontalMovement = _player.GetAxis(HorizontalMovement);
        float verticalMovement = _player.GetAxis(VerticalMovement);
        Vector2 inputVector = new Vector2(horizontalMovement, verticalMovement);
        if (_lastInputVector.magnitude < MinInputAmount && inputVector.magnitude > MinInputAmount) {
            // The player just went from moving nowhere to moving a different direction, so we want to actually move the selection
            _reselectTimer = Mathf.Infinity;
        }
        
        _lastInputVector = inputVector;

        if (_reselectTimer > ReselectionCooldown) {
            // Clamping so that players cannot select outside of the grid
            if (XPos <= 0) {
                horizontalMovement = Mathf.Clamp(horizontalMovement, 0f, Mathf.Infinity);
            }
            if (XPos >= _buttonRows[YPos].Count - 1) {
                horizontalMovement = Mathf.Clamp(horizontalMovement, Mathf.NegativeInfinity, 0f);
            }
            if (YPos >= _buttonRows.Count - 1) {
                verticalMovement = Mathf.Clamp(verticalMovement, 0f, Mathf.Infinity);
            }
            if (YPos <= 0) {
                verticalMovement = Mathf.Clamp(verticalMovement, Mathf.NegativeInfinity, 0f);
            }

            bool moreHorizontalThanVertical = Mathf.Abs(horizontalMovement) > Mathf.Abs(verticalMovement);
            if (moreHorizontalThanVertical) {
                if (horizontalMovement < -MinInputAmount) {
                    MoveLeft();
                } else if (horizontalMovement > MinInputAmount) {
                    MoveRight();
                }
            } else {
                if (verticalMovement < -MinInputAmount) {
                    MoveDown();
                } else if (verticalMovement > MinInputAmount) {
                    MoveUp();
                }
            }
        }
    }

    private void MoveLeft() {
        if (XPos <= 0) return;
        XPos--;
        PlayButtonMoveSound();
        ResetSelection();
    }

    private void MoveRight() {
        if (XPos >= _buttonRows[YPos].Count - 1) return;
        XPos++;
        PlayButtonMoveSound();
        ResetSelection();
    }

    private void MoveDown() {
        if (YPos >= _buttonRows.Count) return;
        YPos++;
        PlayButtonMoveSound();
        ResetSelection();
    }

    private void MoveUp() {
        if (YPos <= 0) return;
        YPos--;
        PlayButtonMoveSound();
        ResetSelection();
    }
    
    /// <summary>
    /// Deselect all unselected buttons and select the selected button
    /// </summary>
    private void ResetSelection() {
        _reselectTimer = 0f;
        for (int y = 0; y < _buttonRows.Count; y++) {
            List<GameButton> buttons = _buttonRows[y];
            for (int x = 0; x < buttons.Count; x++) {
                if (y == YPos && x == XPos) {
                    buttons[x].ToggleSelected(true, ID);
                } else {
                    buttons[x].ToggleSelected(false, ID);
                }
            }
        }
        if (OnAbilityUpdated != null) OnAbilityUpdated();
    }

    #endregion

    private void SetDescriptionText() {
        // TODO
    }

    private void PressSelectedButton() {
        SelectedButton.TryPushButton(ID);
        if (OnAbilityUpdated != null) OnAbilityUpdated();
    }

}
