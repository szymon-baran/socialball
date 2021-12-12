<template>
  <div class="big-data-grid" v-if="isLoggedIn">
    <div class="row">
      <div class="col">
        <h3>Wiadomości</h3>
      </div>
      <div class="col text-right">
        <DxButton
          text="Wyślij wiadomość do swojej drużyny"
          @click="showAddMessagePopup(messageTypeEnum.TEAM)"
          type="default"
          styling-mode="outlined"
          class="mr-2 mt-2"
          v-if="getLoggedInUser.userType === userTypeEnum.TEAM_MANAGEMENT"
        />
        <DxButton
          text="Wyślij wiadomość prywatną"
          @click="showAddMessagePopup(messageTypeEnum.PRIVATE)"
          type="default"
          class="mt-2"
        />
      </div>
    </div>
    <DxTabPanel
      @title-click="tabChanged"
      v-model:selected-index="selectedIndex"
    >
      <DxItem
        :badge="unreadMessagesCount.toString()"
        title="Skrzynka odbiorcza"
      >
        <div>
          <ReceivedMessagesList />
        </div>
      </DxItem>
      <DxItem title="Wysłane wiadomości">
        <div>
          <SentMessagesList />
        </div>
      </DxItem>
      <DxItem title="Otrzymane oferty transferowe">
        <div>
          <ToPlayerTransferOffersList />
        </div>
      </DxItem>
    </DxTabPanel>
  </div>
  <div class="big-data-grid text-center" v-else>
    <h3>Wiadomości są dostępne wyłącznie dla zalogowanych użytkowników!</h3>
    <DxButton
      text="Przejdź na stronę główną"
      type="default"
      styling-mode="outlined"
      @click="routerPushToHome"
    />
  </div>
  <MessageAdd
    v-if="addMessagePopupOptions.isVisible"
    :messageType="addMessagePopupOptions.messageType"
    @close="onAddMessagePopupClose"
  />
</template>
<script>
import DxTabPanel, { DxItem } from "devextreme-vue/tab-panel";
import ReceivedMessagesList from "./ReceivedMessagesList";
import SentMessagesList from "./SentMessagesList";
import ToPlayerTransferOffersList from "../playerTransferOffers/ToPlayerTransferOffersList";
import { mapActions, mapGetters, mapMutations, mapState } from "vuex";
import { userTypeEnum } from "../../enums/userTypeEnum";
import DxButton from "devextreme-vue/button";
import { messageTypeEnum } from "../../enums/messageTypeEnum";
import MessageAdd from "./MessageAdd.vue";

export default {
  name: "MessagesList",
  data() {
    return {
      selectedIndex: 0,
      messageTypeEnum,
      userTypeEnum,
      userTeamId: null,
      addMessagePopupOptions: {
        isVisible: false,
        messageType: null,
      },
    };
  },
  computed: {
    ...mapGetters({
      getLoggedInUser: "authentication/getLoggedInUser",
      isLoggedIn: "authentication/isLoggedIn",
    }),
    ...mapState({
      ...mapState("messages", ["unreadMessagesCount"]),
    }),
  },
  methods: {
    ...mapMutations({
      RESET_MESSAGES: "messages/RESET_MESSAGES",
      RESET_UNREAD_MESSAGES_COUNT: "messages/RESET_UNREAD_MESSAGES_COUNT",
    }),
    ...mapActions({
      setMessages: "messages/setMessages",
      setSentMessages: "messages/setSentMessages",
    }),
    routerPushToHome() {
      this.$router.push("/profile");
    },
    showAddMessagePopup(messageType) {
      this.addMessagePopupOptions.messageType = messageType;
      this.addMessagePopupOptions.isVisible = true;
    },
    onAddMessagePopupClose() {
      this.addMessagePopupOptions.messageType = null;
      this.addMessagePopupOptions.isVisible = false;
    },
    tabChanged() {
      this.RESET_MESSAGES();
      switch (this.selectedIndex) {
        case 0:
          this.RESET_UNREAD_MESSAGES_COUNT();
          this.setMessages(this.getLoggedInUser.id);
          break;
        case 1:
          this.setSentMessages(this.getLoggedInUser.id);
          break;
      }
    },
  },
  components: {
    DxTabPanel,
    DxItem,
    DxButton,
    ReceivedMessagesList,
    SentMessagesList,
    ToPlayerTransferOffersList,
    MessageAdd,
  },
  mounted() {
    this.setMessages(this.getLoggedInUser.id);
  },
  beforeUnmount() {
    this.RESET_MESSAGES();
    this.RESET_UNREAD_MESSAGES_COUNT();
  },
};
</script>
