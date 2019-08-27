using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTablePositions
{
    /// <summary>
    /// Stores the eight possible main card positions here,
    /// using the format of SEAT_POS_POSITION, i.e. FIRST_POS_2
    /// corresponds to the second card at the first position's vector location.
    /// The quaternions correspond to the possible rotations and follow
    /// the same variable name format.
    /// 
    /// Positions begin at one and go clockwise around the table for reference.
    /// 
    /// THE DEFAULT VALUES ARE THE CENTER OF THE TABLE
    /// </summary>
    public static Vector3 FIRST_POS_1 = new Vector3(-18.5f, 3.61f, -2.78f);
    public static Vector3 FIRST_POS_2 = new Vector3(-17.45f, 3.61f, -2.78f);
    public static Quaternion FIRST_ROT_1 = Quaternion.Euler(-90f, 90f, 0f);
    public static Quaternion FIRST_ROT_2 = Quaternion.Euler(-90f, 90f, 0f);

    public static Vector3 SECOND_POS_1 = new Vector3(-21.5f, 3.61f, 1.74f);
    public static Vector3 SECOND_POS_2 = new Vector3(-21.5f, 3.61f, 0.744f);
    public static Quaternion SECOND_ROT_1 = Quaternion.Euler(-90f, 0f, 0f);
    public static Quaternion SECOND_ROT_2 = Quaternion.Euler(-90f, 0f, 0f);

    public static Vector3 THIRD_POS_1 = new Vector3(-17.45f, 3.61f, 4.5f);
    public static Vector3 THIRD_POS_2 = new Vector3(-18.5f, 3.61f, 4.5f);
    public static Quaternion THIRD_ROT_1 = Quaternion.Euler(-90f, 90f, 0f);
    public static Quaternion THIRD_ROT_2 = Quaternion.Euler(-90f, 90f, 0f);

    public static Vector3 FOURTH_POS_1 = new Vector3(-14.32f, 3.61f, 0.384f);
    public static Vector3 FOURTH_POS_2 = new Vector3(-14.32f, 3.61f, 1.38f);
    public static Quaternion FOURTH_ROT_1 = Quaternion.Euler(-90f, 0f, 0f);
    public static Quaternion FOURTH_ROT_2 = Quaternion.Euler(-90f, 0f, 0f);

    // COMMUNITY CARD POSITIONS
    public static Vector3 FLOP_POS_1 = new Vector3(-20.11398f, 3.64f, 1.094045f);
    public static Quaternion FLOP_ROT_1 = Quaternion.Euler(-90f, 0f, -90f);

    public static Vector3 FLOP_POS_2 = new Vector3(-19.0228f, 3.64f, 1.094045f);
    public static Quaternion FLOP_ROT_2 = Quaternion.Euler(-90f, 0f, -90f);

    public static Vector3 FLOP_POS_3 = new Vector3(-17.93286f, 3.64f, 1.094045f);
    public static Quaternion FLOP_ROT_3 = Quaternion.Euler(-90f, 0f, -90f);

    public static Vector3 FLOP_POS_4 = new Vector3(-16.81293f, 3.64f, 1.094045f);
    public static Quaternion FLOP_ROT_4 = Quaternion.Euler(-90f, 0f, -90f);

    public static Vector3 FLOP_POS_5 = new Vector3(-15.703f, 3.64f, 1.094045f);
    public static Quaternion FLOP_ROT_5 = Quaternion.Euler(-90f, 0f, -90f);

    // DEFAULT VALUES
    public static Vector3 DEFAULT_CARD_POS = new Vector3(-1.505f, 3.64f, 1.51f);
    public static Quaternion DEFAULT_CARD_ROT = Quaternion.Euler(-90f, 0f, 0f);

    public static Vector3 DEFAULT_DECK_POS = new Vector3(-1.505f, 3.652f, 0.486f);
    public static Quaternion DEFAULT_DECK_ROT = Quaternion.Euler(-90f, 0f, 0f);
}
