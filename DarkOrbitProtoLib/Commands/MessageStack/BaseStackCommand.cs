using System.Text;

namespace DarksideApi.DarkOrbit.Commands.MessageStack
{
    public class BaseStackCommand : Command
    {
        public new const short Id = 27145;
        public string stringBasekey;
        public BaseKey enumBaseKey;
        public List<ArgStackCommand> argList;
        public int type;

        public BaseStackCommand()
        {
            this.stringBasekey = string.Empty;
            this.argList = [];
        }

        public override void Read(ByteArray reader)
        {
            this.stringBasekey = reader.ReadUTF();
            this.type = reader.ReadInt();
            this.type = this.type >>> 7 | this.type << 25;
            this.enumBaseKey = (BaseKey)reader.ReadShort();
            var i = 0;
            var max = reader.ReadByte();
            while (i < max)
            {
                reader.ReadShort();
                var arg = new ArgStackCommand();
                arg.Read(reader);
                this.argList.Add(arg);
            }     
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"BaseStackCommand: {this.stringBasekey}");
            sb.AppendLine($"EnumBaseKey: {this.enumBaseKey}");
            sb.AppendLine($"Type: {this.type}");
            sb.AppendLine("Arguments:");
            foreach (ArgStackCommand arg in this.argList)
            {
                sb.AppendLine(arg.ToString());
            }
            return sb.ToString();
        }

        public enum BaseKey
        {

            empty,

            eqt_error_generic,

            info_text_switch_hangar_station_only,

            hangar_switchship_cooldown,

            info_text_switch_active_hangar_success,

            eqt_error_playerMayNotEquipWithoutHangarFacility,

            banking_error_no_Uridium,

            error_pet_name_length,

            error_pet_name_char,

            items_hangarSlot_max_favs,

            err_no_pearls,

            err_no_gold,

            error_unknown,

            err_support,

            err_eventitem_not_available,

            items_booster_bought,

            items_drone_formation_already_in_use,

            label_empty_pet_tab,

            pet_gear_shop_level_limit,

            pet_gear_requirement,

            err_no_ship,

            items_error_ship_owned,

            skylab_message_transport_isNotActive,

            items_shop_buy_error_unique,

            buy_pet_invalidFuelAmount,

            Internal_error,

            hangar_overview_no_pet,

            shop_item_not_found,

            items_shop_buy_error_infected,

            captain_popup_nova_roster_full,

            skylab_message_upgrade_maximumLevel,

            skylab_message_upgrade_basemoduleLevel,

            skylab_message_upgrade_credits,

            skylab_message_upgrade_prometium,

            skylab_message_upgrade_endurium,

            skylab_message_upgrade_terbium,

            skylab_history_module_activate_notBuiltYet,

            skylab_history_module_deactivated,

            skylab_history_module_activated,

            skylab_history_module_activate_lowpower,

            skylab_message_upgrade_success,

            skylab_history_module_upgrade_lowpower,

            skylab_message_upgrade_inProgress,

            skylab_history_upgrade_canceled,

            skylab_message_upgrade_uridium,

            skylab_history_upgrade_instant,

            skylab_history_upgrade_finished,

            skylab_message_transport_notEnoughUridium,

            skylab_message_transport_alreadyActive,

            skylab_message_transport_requestedOreAmountIsZero,

            skylab_message_transport_notEnoughSpaceInShipStorage,

            skylab_message_transport_isEmpty,

            skylab_history_transfer_ship_started,

            skylab_history_transfer_ship_finished,

            messaging_errorLayer_restrictionUnknownUser_text,

            messaging_errorLayer_spam_title,

            messaging_errorLayer_spam_text,

            messaging_errorLayer_noRecipient_title,

            messaging_errorLayer_noRecipient_text,

            messaging_errorLayer_tooManyRecipients_title,

            messaging_errorLayer_tooManyRecipients_text,

            messaging_captcha_msg_info,

            messaging_errorLayer_blacklisted_title,

            messaging_errorLayer_blacklisted_text,

            messaging_errorLayer_blocked_title,

            messaging_errorLayer_blocked_text,

            messaging_successLayer_messageSent_title,

            messaging_successLayer_messageSent_text,

            messaging_errorLayer_system_title,

            messaging_errorLayer_system_text,

            messaging_errorLayer_notesLimitReached_title,

            messaging_errorLayer_notesLimitReached_text,

            messaging_successLayer_messageSavedAsNote_title,

            messaging_successLayer_messageSavedAsNote_text,

            messaging_errorLayer_subjectTooShort_title,

            messaging_errorLayer_subjectTooShort_text,

            messaging_errorLayer_subjectTooLong_title,

            messaging_errorLayer_subjectTooLong_text,

            messaging_errorLayer_bodyTooShort_title,

            messaging_errorLayer_bodyTooShort_text,

            messaging_errorLayer_bodyTooLong_title,

            messaging_errorLayer_bodyTooLong_text,

            auction_error_generic,

            drone_have_max,

            items_shop_error_ship_contains_ship,

            items_design_already_in_use,

            auction_buy_instant_invalid,

            too_fast,

            not_enough_uridium,

            auction_bid_toofast,

            auction_buy_toofast,

            auction_bid_max,

            captcha,

            auction_bid_credits_to_little,

            auction_bid_minBid_not_reached,

            auction_bid_saved,

            err_user_in_clan,

            clan_errmsg_required,

            clan_errmsg_to_long,

            clan_errmsg_desc_to_long,

            clan_errmsg_clanname_exists,

            clan_name_not_allowed,

            clan_errmsg_clantag_exists,

            clan_errmsg_clantag_not_allowed,

            clan_user_2dayblock,

            clan_errmsg_user_not_enough_money,

            not_enough_credits,

            err_select_clan,

            err_clan_apply,

            err_clan_apply_send,

            err_clan_exist,

            err_clan_noRights,

            err_clan_full,

            clan_not_your_clan,

            clan_user_in_clan,

            err_clan_decline,

            err_clan_no_access,

            clan_remove_member,

            clan_errmsg_leave_at_station,

            clan_changed_tax,

            clan_err_wrong_value,

            err_clan_user_not_in_clan,

            err_clan_no_money,

            clan_message_error_payout_userMax_reached,

            success_clan_payout,

            clan_msg_send,

            clan_msg_delete,

            clan_msg_all_delete,

            success_clan_change_descr,

            err_no_clanname,

            err_clantag_exist,

            clan_tag_not_allowed,

            err_clannameChange2Days,

            err_clan_other_fractionID,

            success_change_data,

            err_clan_no_rank,

            err_clan_no_change_rank,

            success_clan_change_rank,

            err_clan_emptyRankNotAllowed,

            err_clan_maxRank,

            err_clan_rankExist,

            clan_new_rank_insert,

            clan_delete_my_rank_success,

            success_clan_edit_rank,

            firmChange_notavailable,

            err_clan_firmchange_closeQuest,

            firmChange_done,

            err_diplo_default,

            clan_message_report_to_admin,

            err_clan_you_are_leader,

            success_clan_changeLeader,

            err_clan_noClanID,

            err_diplo_not_with_own_clan,

            err_clan_diploRequest_exist,

            err_diplo_war_is_running,

            err_diplo_applyIsRunning,

            err_diplo_isRunning,

            clan_newDiploOffer,

            err_clan_firmchange_notSpacestation_notMapX1,

            success_war_is_closed,

            diplo_new_allow,

            diplo_new_deny,

            diplo_request_delete,

            diplo_end_success,

            change_pilotname_error_signs_number,

            change_pilotname_error_signs_illegal,

            change_pilotname_error_user_blocked,

            change_pilotname_error_inUse,

            change_pilotname_success,

            messaging_confirmLayer_blockAllInvites_title,

            messaging_successLayer_blockAllInvites_text,

            messaging_successLayer_unblockAllInvites_text,

            eqt_error_playerMayNotEquip2,

            special_generic_hint,

            error_can_not_equip_to_slot,

            captain_error_unhandled,

            captain_error_not_near_spacestation,

            captain_error_captain_no_energy,

            captain_popup_dismiss_active_nova,

            captain_hangar_error_perk_lvl_cap,

            captain_hangar_error_not_enough_resources,

            popup_save_success_title,

            popup_save_success_description,

            popup_load_success_title,

            popup_load_success_description,

            eqt_error_playerMayNotRepair,

            err_changing_company_tips
        }
    }
}
