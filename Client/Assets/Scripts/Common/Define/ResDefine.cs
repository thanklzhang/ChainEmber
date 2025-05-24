/*
 * generate by tool
*/
using System.Collections;
using System;
using System.Collections.Generic;
namespace Config
{
	public enum ResIds
	{
		
		LoginUI = 15000001,
        
		LobbyUI = 15000002,
        
		HeroListUI = 15000003,
        
		BattleUI = 15000004,
        
		LoadingUI = 15000005,
        
		HeroInfoUI = 15000006,
        
		ConfirmUI = 15000007,
        
		MainTaskUI = 15000008,
        
		MainTaskStageUI = 15000009,
        
		BattleResultUI = 15000010,
        
		TitleBarUI = 15000011,
        
		TeamRoomListUI = 15000012,
        
		TeamRoomInfoUI = 15000013,
        
		TipsUI = 15000014,
        
		SelectHeroUI = 15000015,
        
		BattleReviveUI = 15000016,
        
		BattleReplaceSkillUI = 15000501,
        
		BattleReplaceHeroUI = 15000502,
        
		hero_100 = 15001001,
        
		icon_money = 15002001,
        
		cursor_001 = 15003001,
        
		cursor_002 = 15003002,
        
		cursor_select_attack_001 = 15003003,
        
		cursor_normal_005 = 15003004,
        
		cursor_attack_001 = 15003005,
        
		icon_buff_common_01 = 15003101,
        
		icon_buff_common_02 = 15003102,
        
		icon_buff_common_03 = 15003103,
        
		icon_buff_common_04 = 15003104,
        
		icon_buff_common_05 = 15003105,
        
		icon_buff_common_06 = 15003106,
        
		icon_buff_common_07 = 15003107,
        
		icon_buff_common_08 = 15003108,
        
		icon_buff_common_09 = 15003109,
        
		icon_buff_common_10 = 15003110,
        
		icon_buff_common_11 = 15003111,
        
		icon_buff_common_12 = 15003112,
        
		icon_buff_common_13 = 15003113,
        
		icon_buff_common_14 = 15003114,
        
		icon_buff_common_15 = 15003115,
        
		icon_buff_common_16 = 15003116,
        
		icon_buff_common_17 = 15003117,
        
		icon_buff_common_18 = 15003118,
        
		icon_buff_common_19 = 15003119,
        
		icon_buff_common_20 = 15003120,
        
		icon_buff_common_21 = 15003121,
        
		icon_buff_common_22 = 15003122,
        
		icon_buff_common_23 = 15003123,
        
		icon_buff_common_24 = 15003124,
        
		icon_buff_common_25 = 15003125,
        
		icon_buff_common_26 = 15003126,
        
		icon_buff_hero_0001 = 15003201,
        
		icon_buff_hero_0002 = 15003202,
        
		icon_buff_hero_0003 = 15003203,
        
		icon_buff_hero_0004 = 15003204,
        
		icon_buff_hero_0005 = 15003205,
        
		icon_buff_hero_0006 = 15003206,
        
		icon_buff_hero_0007 = 15003207,
        
		icon_buff_hero_0008 = 15003208,
        
		icon_buff_hero_0009 = 15003209,
        
		icon_buff_hero_0010 = 15003210,
        
		icon_buff_hero_0011 = 15003211,
        
		icon_buff_hero_0012 = 15003212,
        
		icon_buff_hero_0013 = 15003213,
        
		icon_buff_hero_0014 = 15003214,
        
		icon_buff_hero_0015 = 15003215,
        
		icon_buff_hero_0016 = 15003216,
        
		icon_buff_hero_0017 = 15003217,
        
		icon_buff_hero_0018 = 15003218,
        
		icon_buff_hero_0019 = 15003219,
        
		icon_buff_hero_0020 = 15003220,
        
		icon_buff_hero_0021 = 15003221,
        
		icon_buff_hero_0022 = 15003222,
        
		icon_buff_hero_0023 = 15003223,
        
		icon_buff_hero_0024 = 15003224,
        
		icon_buff_hero_0025 = 15003225,
        
		icon_buff_hero_0026 = 15003226,
        
		icon_buff_hero_0027 = 15003227,
        
		icon_buff_hero_0028 = 15003228,
        
		icon_buff_hero_0029 = 15003229,
        
		icon_buff_hero_0030 = 15003230,
        
		icon_buff_hero_0031 = 15003231,
        
		icon_buff_hero_0032 = 15003232,
        
		icon_buff_hero_0033 = 15003233,
        
		icon_buff_hero_0034 = 15003234,
        
		icon_buff_hero_0035 = 15003235,
        
		icon_buff_hero_0036 = 15003236,
        
		icon_buff_hero_0037 = 15003237,
        
		icon_buff_hero_0038 = 15003238,
        
		icon_buff_hero_0039 = 15003239,
        
		icon_buff_hero_0040 = 15003240,
        
		icon_buff_hero_0041 = 15003241,
        
		icon_buff_hero_0042 = 15003242,
        
		icon_buff_hero_0043 = 15003243,
        
		icon_buff_hero_0044 = 15003244,
        
		icon_buff_hero_0045 = 15003245,
        
		icon_buff_hero_0046 = 15003246,
        
		icon_buff_hero_0047 = 15003247,
        
		icon_buff_hero_0048 = 15003248,
        
		icon_buff_hero_0049 = 15003249,
        
		icon_buff_hero_0050 = 15003250,
        
		icon_buff_hero_0051 = 15003251,
        
		icon_buff_hero_0052 = 15003252,
        
		icon_buff_hero_0053 = 15003253,
        
		icon_buff_hero_0054 = 15003254,
        
		icon_buff_hero_0055 = 15003255,
        
		icon_buff_hero_0056 = 15003256,
        
		icon_buff_hero_0057 = 15003257,
        
		icon_buff_hero_0058 = 15003258,
        
		icon_buff_hero_0059 = 15003259,
        
		icon_buff_hero_0060 = 15003260,
        
		icon_buff_hero_0061 = 15003261,
        
		img_avatar_001 = 15003501,
        
		img_avatar_002 = 15003502,
        
		img_avatar_003 = 15003503,
        
		img_avatar_004 = 15003504,
        
		img_avatar_005 = 15003505,
        
		img_avatar_006 = 15003506,
        
		img_avatar_007 = 15003507,
        
		img_avatar_008 = 15003508,
        
		img_avatar_009 = 15003509,
        
		img_all_body_001 = 15003601,
        
		img_all_body_002 = 15003602,
        
		img_all_body_003 = 15003603,
        
		img_all_body_004 = 15003604,
        
		img_attr_attack = 15003701,
        
		img_attr_defend = 15003702,
        
		img_attr_max_health = 15003703,
        
		img_attr_attack_speed = 15003704,
        
		img_attr_move_speed = 15003705,
        
		img_attr_attack_range = 15003706,
        
		img_skill_001 = 15003751,
        
		img_skill_002 = 15003752,
        
		img_skill_003 = 15003753,
        
		img_skill_004 = 15003754,
        
		img_skill_005 = 15003755,
        
		item0 = 15004001,
        
		item1 = 15004002,
        
		item2 = 15004003,
        
		bg_01 = 15005000,
        
		bg_02 = 15005001,
        
		bg_03 = 15005002,
        
		bg_04 = 15005003,
        
		audio_click_001 = 15006001,
        
		bgm_battle_001 = 15006002,
        
		bgm_001 = 15006003,
        
		bgm_002 = 15006004,
        
		eft_select_entity_01 = 15008001,
        
		eft_buff_fire_follow = 15008002,
        
		eft_skill_projectile_001 = 15008003,
        
		eft_skill_cal_001 = 15008004,
        
		eft_skill_cal_002 = 15008005,
        
		eft_hit_001 = 15008006,
        
		eft_arrow_001 = 15008007,
        
		eft_ring_dart_001 = 15008008,
        
		eft_move_trail = 15008009,
        
		eft_buff_lose_health_halo = 15008010,
        
		eft_health_impact = 15008011,
        
		eft_buff_gray_halo = 15008012,
        
		eft_add_health = 15008013,
        
		eft_stun_range = 15008014,
        
		eft_stun_circle = 15008015,
        
		eft_knife_light = 15008016,
        
		eft_buff_001 = 15008017,
        
		eft_buff_002 = 15008018,
        
		eft_impact_001 = 15008019,
        
		eft_impact_002 = 15008020,
        
		eft_skill_projectile_002 = 15008021,
        
		eft_buff_003 = 15008022,
        
		eft_buff_004 = 15008023,
        
		eft_buff_005 = 15008024,
        
		eft_buff_006 = 15008025,
        
		eft_buff_007 = 15008026,
        
		eft_knife_hit_001 = 15008027,
        
		eft_knife_hit_002 = 15008028,
        
		eft_range_001 = 15008029,
        
		eft_explosion_002 = 15008030,
        
		eft_explosion_003 = 15008031,
        
		eft_buff_link_001 = 15008032,
        
		eft_skill_director_001 = 15008101,
        
		eft_skill_director_002 = 15008102,
        
		eft_skill_track_001 = 15008201,
        
		eft_skill_track_002 = 15008202,
        
		eft_skill_track_003 = 15008203,
        
		battle_scene_001 = 15010001,
        
		scene_wuxia_002 = 15010002,
        
		scene_ogre_001 = 15010003,
        
		shangguanling_battle_001 = 15011001,
        
		shangguanling_battle_002 = 15011002,
        
		shangguanling_battle_003 = 15011003,
        
		shangguanling_battle_004 = 15011004,
        
		tangyi_battle_003 = 15011021,
        
		role_001 = 15011031,
        
		role_002 = 15011032,
        
		role_003 = 15011033,
        
		role_004 = 15011034,
        
		role_005 = 15011035,
        
		role_006 = 15011036,
        
		role_007 = 15011037,
        
		role_008 = 15011038,
        
		monster_001 = 15011101,
        
		monster_002 = 15011102,
        
		monster_boss_101 = 15011103,
        
		monster_201 = 15011104,
        
		monster_202 = 15011105,
        
		monster1_battle = 15012001,
        
		daoba_battle_030 = 15013001,
        
		OutlineEffect = 15014001,
        
		Battle_02_Desert = 15015001,
        
		LoginScene = 15015100,
        
		LobbyScene = 15015101,
        
	
	}  
}