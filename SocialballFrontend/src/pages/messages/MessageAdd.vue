<template>
  <div>
    <DxPopup
      v-model:visible="popupVisible"
      :drag-enabled="false"
      :close-on-outside-click="false"
      :show-close-button="false"
      :show-title="true"
      :width="650"
      :height="650"
      container=".dx-viewport"
      :title="getTitle()"
      :shading="false"
    >
      <DxToolbarItem
        widget="dxButton"
        toolbar="bottom"
        location="after"
        :options="sendButtonOptions"
      />
      <DxToolbarItem
        widget="dxButton"
        toolbar="bottom"
        location="before"
        :options="closeButtonOptions"
      />
      <form>
        <DxValidationGroup :ref="groupRefKey">
          <div class="row">
            <div class="col" v-if="messageType === messageTypeEnum.PRIVATE">
              <label for="toUserIdSelectBox" class="form-label">Odbiorca</label>
              <DxSelectBox
                :dataSource="users"
                value-expr="id"
                display-expr="name"
                v-model="ToUserId"
                id="toUserIdSelectBox"
                :read-only="isUserSelectorReadOnly"
              />
            </div>
            <div class="col">
              <label for="titleTextBox" class="form-label"
                >Tytuł wiadomości</label
              >
              <DxTextBox id="titleTextBox" v-model="Subject" />
            </div>
          </div>
          <div class="row mt-4">
            <div class="col">
              <label for="contentEditor" class="form-label"
                >Treść wiadomości</label
              >
              <DxHtmlEditor height="400" id="contentEditor" v-model="Content">
                <DxToolbar>
                  <DxItem :accepted-values="sizeValues" name="size" />
                  <DxItem :accepted-values="fontValues" name="font" />
                  <DxItem name="bold" />
                  <DxItem name="italic" />
                  <DxItem name="strike" />
                  <DxItem name="underline" />
                  <DxItem name="separator" />
                  <DxItem name="alignLeft" />
                  <DxItem name="alignCenter" />
                  <DxItem name="alignRight" />
                  <DxItem name="alignJustify" />
                  <DxItem name="separator" />
                  <DxItem name="orderedList" />
                  <DxItem name="bulletList" />
                </DxToolbar>
              </DxHtmlEditor>
            </div>
          </div>
          <div class="row">
            <div class="col">
              <DxValidationSummary />
            </div>
          </div>
        </DxValidationGroup>
      </form>
    </DxPopup>
  </div>
</template>
<script>
import { DxPopup, DxToolbarItem } from "devextreme-vue/popup";
import { DxHtmlEditor, DxToolbar, DxItem } from "devextreme-vue/html-editor";
import DxTextBox from "devextreme-vue/text-box";
import DxSelectBox from "devextreme-vue/select-box";
import { mapActions, mapGetters, mapMutations } from "vuex";
import { createHelpers } from "vuex-map-fields";
const { mapFields } = createHelpers({
  getterType: "messages/getField",
  mutationType: "messages/updateField",
});
import DxValidationGroup from "devextreme-vue/validation-group";
import DxValidationSummary from "devextreme-vue/validation-summary";
import { useToast } from "vue-toastification";
import { messageTypeEnum } from "../../enums/messageTypeEnum";

export default {
  name: "MessageAdd",
  props: {
    messageType: {
      type: Number,
      required: true,
    },
    userIdFromProfile: {
      type: String,
      required: false,
    },
  },
  data() {
    return {
      messageTypeEnum,
      users: [],
      popupVisible: false,
      isUserSelectorReadOnly: false,
      sizeValues: ["8pt", "10pt", "12pt", "14pt", "18pt", "24pt", "36pt"],
      fontValues: [
        "Arial",
        "Courier New",
        "Georgia",
        "Impact",
        "Lucida Console",
        "Tahoma",
        "Times New Roman",
        "Verdana",
      ],
      sendButtonOptions: {
        icon: "email",
        text: "Wyślij",
        onClick: () => this.handleSubmit(),
      },
      closeButtonOptions: {
        text: "Zamknij",
        onClick: () => {
          this.popupVisible = false;
          this.$emit("close");
        },
        type: "normal",
      },
      groupRefKey: "targetGroup",
    };
  },
  computed: {
    ...mapGetters({
      getLoggedInUser: "authentication/getLoggedInUser",
      getPlayerDetails: "players/getPlayerDetails",
    }),
    ...mapFields([
      "message.FromUserId",
      "message.ToTeamId",
      "message.ToUserId",
      "message.Subject",
      "message.Content",
      "message.SentOn",
      "message.MessageType",
    ]),
    validationGroup: function() {
      return this.$refs[this.groupRefKey].instance;
    },
  },
  methods: {
    ...mapActions({
      sendMessage: "messages/sendMessage",
      getUsersToLookup: "messages/getUsersToLookup",
      getUserDataByUserId: "players/getUserDataByUserId",
    }),
    ...mapMutations({
      RESET_TEAM_MESSAGE_FORM: "messages/RESET_TEAM_MESSAGE_FORM",
    }),
    async handleSubmit() {
      let validationResult = this.validationGroup.validate();
      if (validationResult.isValid) {
        await this.sendMessage();
        useToast().success("Wiadomość została wysłana pomyślnie!");
        this.popupVisible = false;
        this.$emit("close");
      }
    },
    getTitle() {
      switch (this.messageType) {
        case messageTypeEnum.PRIVATE:
          return "Wyślij wiadomość prywatną";
        case messageTypeEnum.TEAM:
          return "Wyślij wiadomość do swojej drużyny";
        default:
          return "";
      }
    },
  },
  components: {
    DxPopup,
    DxToolbarItem,
    DxHtmlEditor,
    DxToolbar,
    DxItem,
    DxTextBox,
    DxSelectBox,
    DxValidationGroup,
    DxValidationSummary,
  },
  mounted() {
    this.popupVisible = true;
    this.FromUserId = this.getLoggedInUser.id;
    this.MessageType = this.messageType;
    if (this.messageType === messageTypeEnum.PRIVATE) {
      this.getUsersToLookup().then((response) => {
        this.users = response.data;
      });
    } else if (this.messageType === messageTypeEnum.TEAM) {
      this.getUserDataByUserId(this.FromUserId).then(() => {
        this.ToTeamId = this.getPlayerDetails.TeamId;
      });
    }
    if (this.userIdFromProfile) {
      this.isUserSelectorReadOnly = true;
      this.ToUserId = this.userIdFromProfile;
    }
  },
  beforeUnmount() {
    this.RESET_TEAM_MESSAGE_FORM();
  },
};
</script>
