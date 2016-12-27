/***********************************************
 *
 *   PangServ - Pangya Server Emulator       
 *   Copyright © 2016 PangServ-Team
 *
 ***********************************************
 * Defaults.
 ***********************************************/
using System.Runtime.InteropServices;

namespace defs
{

    enum E_GAME_TYPE
    {
        GAME_TYPE_VERSUS_STROKE = 0x00,
        GAME_TYPE_VERSUS_MATCH,
        GAME_TYPE_CHAT_ROOM,
        GAME_TYPE_03,
        GAME_TYPE_TOURNEY_TOURNEY,      // 30 Players tournament
        GAME_TYPE_TOURNEY_TEAM,         // 30 Players team tournament
        GAME_TYPE_TOURNEY_GUILD,        // Guild battle
        GAME_TYPE_BATTLE_PANG_BATTLE,   // Pang Battle
        GAME_TYPE_08,                   // Public My Room
        GAME_TYPE_09,
        GAME_TYPE_0A,
        GAME_TYPE_0B,
        GAME_TYPE_0C,
        GAME_TYPE_0D,
        GAME_TYPE_CHIP_IN_PRACTICE,
        GAME_TYPE_0F,                   // Playing for the first time
        GAME_TYPE_10,                   // Learn with caddie
        GAME_TYPE_11,                   // Stroke
        GAME_TYPE_12,                   // This is Chaos!
        GAME_TYPE_HOLE_REPEAT,          // This is Chaos!
        GAME_TYPE_14                    // Grand Prix
    }

    enum E_GAME_MODE
    {
        GAME_MODE_FRONT = 0x0,
        GAME_MODE_BACK,
        GAME_MODE_RANDOM,
        GAME_MODE_SHUFFLE,
        GAME_MODE_REPEAT
    }

    enum E_PLAYER_ACTION
    {
        PLAYER_ACTION_NULL = 0x0,
        PLAYER_ACTION_APPEAR = 0x4,
        PLAYER_ACTION_SUB,
        PLAYER_ACTION_MOVE,
        PLAYER_ACTION_ANIMATION
    }

    enum E_PLAYER_ACTION_SUB
    {
        PLAYER_ACTION_SUB_STAND = 0x0,
        PLAYER_ACTION_SUB_SIT,
        PLAYER_ACTION_SUB_SLEEP
    }

    enum E_SHOT_TYPE
    {
        SHOT_TYPE_NORMAL = 0x02,
        SHOT_TYPE_OB,
        SHOT_TYPE_INHOLE,
        SHOT_TYPE_UNKNOW = 0xFF
    }

    enum E_CLUB_TYPE
    {
        CLUB_TYPE_1W = 0,
        CLUB_TYPE_2W,
        CLUB_TYPE_3W,
        CLUB_TYPE_2L,
        CLUB_TYPE_3L,
        CLUB_TYPE_4L,
        CLUB_TYPE_5L,
        CLUB_TYPE_6L,
        CLUB_TYPE_7L,
        CLUB_TYPE_8L,
        CLUB_TYPE_9L,
        CLUB_TYPE_PW,
        CLUB_TYPE_SW,
        CLUB_TYPE_PT
    }

    enum E_ITEM_TYPE
    {
        ITEM_TYPE_CHARACTER = 0x04,
        ITEM_TYPE_FASHION = 0x08,
        ITEM_TYPE_CLUB = 0x10,
        ITEM_TYPE_AZTEC = 0x14,
        ITEM_TYPE_ITEM1 = 0x18,
        ITEM_TYPE_ITEM2 = 0x1A,
        ITEM_TYPE_CADDIE = 0x1C,
        ITEM_TYPE_CADDIE_ITEM = 0x20,
        ITEM_TYPE_ITEM_SET = 0x24,
        ITEM_TYPE_CADDIE_ITEM2 = 0x34,
        ITEM_TYPE_SKIN = 0x38,
        ITEM_TYPE_TITLE,
        ITEM_TYPE_HAIR_COLOR1 = 0x3C,
        ITEM_TYPE_HAIR_COLOR2 = 0x3E,
        ITEM_TYPE_MASCOT = 0x40,
        ITEM_TYPE_FURNITURE = 0x48,
        ITEM_TYPE_CARD_SET = 0x7C,
        ITEM_TYPE_UNKNOW = 0xFF
    }

    enum E_RANK
    {
        ROOKIE_F = 0x00,
        ROOKIE_E,
        ROOKIE_D,
        ROOKIE_C,
        ROOKIE_B,
        ROOKIE_A,
        BEGINNER_E,
        BEGINNER_D,
        BEGINNER_C,
        BEGINNER_B,
        BEGINNER_A,
        JUNIOR_E,
        JUNIOR_D,
        JUNIOR_C,
        JUNIOR_B,
        JUNIOR_A,
        SENIOR_E,
        SENIOR_D,
        SENIOR_C,
        SENIOR_B,
        SENIOR_A,
        AMATEUR_E ,
        AMATEUR_D ,
        AMATEUR_C ,
        AMATEUR_B ,
        AMATEUR_A ,
        SEMI_PRO_E,
        SEMI_PRO_D,
        SEMI_PRO_C,
        SEMI_PRO_B,
        SEMI_PRO_A,
        PRO_E,
        PRO_D,
        PRO_C,
        PRO_B,
        PRO_A,
        NATIONAL_PRO_E,
        NATIONAL_PRO_D,
        NATIONAL_PRO_C,
        NATIONAL_PRO_B,
        NATIONAL_PRO_A,
        WORLD_PRO_E,
        WORLD_PRO_D,
        WORLD_PRO_C,
        WORLD_PRO_B,
        WORLD_PRO_A,
        MASTER_E,
        MASTER_D,
        MASTER_C,
        MASTER_B,
        MASTER_A,
        TOP_MASTER_E,
        TOP_MASTER_D,
        TOP_MASTER_C,
        TOP_MASTER_B,
        TOP_MASTER_A,
        WORLD_MASTER_E,
        WORLD_MASTER_D,
        WORLD_MASTER_C,
        WORLD_MASTER_B,
        WORLD_MASTER_A,
        LEGEND_E,
        LEGEND_D,
        LEGEND_C,
        LEGEND_B,
        LEGEND_A,
        INFINITY_LEGEND_E,
        INFINITY_LEGEND_D,
        INFINITY_LEGEND_C,
        INFINITY_LEGEND_B,
        INFINITY_LEGEND_A
    }

    enum E_CREATE_GAME_RESULT
    {
        CREATE_GAME_RESULT_SUCCESS = 0x00,
        CREATE_GAME_RESULT_FULL = 0x02,
        CREATE_GAME_ROOM_DONT_EXISTS,
        CREATE_GAME_INCORRECT_PASSWORD,
        CREATE_GAME_INVALID_LEVEL,
        CREATE_GAME_CREATE_FAILED = 0x07,
        CREATE_GAME_ALREADY_STARTED,
        CREATE_GAME_CREATE_FAILED2,
        CREATE_GAME_NEED_REGISTER_WITH_GUILD = 0x0D,
        CREATE_GAME_PANG_BATTLE_INSSUFICENT_PANGS = 0x0F,
        CREATE_GAME_APPROACH_INSSUFICENT_PANGS = 0x11,
        CREATE_GAME_CANT_CREATE
    }

    class PlayerUID
    {
        ushort a, b, c, typ;
        public uint id;
        public string login;

        [StructLayout(LayoutKind.Explicit)]
        struct TIffId
        {
            [FieldOffset(0)]
            public uint id;
            [FieldOffset(0)]
            public byte a;
            [FieldOffset(1)]
            public byte b;
            [FieldOffset(2)]
            public byte c;
            [FieldOffset(3)]
            public byte typ;
        }

        void SetID(int id_)
        {
            id = (uint)id_;
        }

        byte WriteGameCreateResult(E_CREATE_GAME_RESULT gameCreateResult)
        {
            return ((byte)gameCreateResult);
        }
    }
}
