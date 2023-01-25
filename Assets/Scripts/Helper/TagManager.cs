using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public static class TagManager 
{
    public static string HORIZONTAL_AXIS = "Horizontal";
    public static string VERTICAL_AXIS = "Vertical";

    public static string PLAYER_MOVE_ANIMATION_PARAMETR = "moveSpeed";
    public static string PLAYER_JUMP_ANIMATION_PARAMETR = "isGrounded";
    public static string PLAYER_DOUBLE_JUMP_TRIGGER = "doubleJump";
    public static string PLAYER_SHOOT_TRIGGER = "shoot";

    public static string PLAYER_TAG = "Player";
    public static string ENEMY_TAG = "Enemy";

    public static string ENEMY_MOVE_SPEED_ANIM_PARAMETR = "speed";
    public static string ENEMY_CHASING_ANIM_PARAMETR = "isChasing";

    public static string PICK_UP_MAX_HEALTH_TRIGGER = "maxHealth";

    public static string DOOR_IS_OPEN_PARAMETR = "doorOpen";

    public static string BOSS_TAG = "Boss";
    public static string BOSS_VANISH_TRIGGER = "Vanish";

    public static string PLAYER_BULLET_TAG = "PlayerBullet";

    public static string MAIN_MENU_SCENE_NAME = "MainMenu";
    public static string LEVEL_1_SCENE_NAME = "Level1";

    public static string WIN_LEVEL_NAME = "WinGame";
}
