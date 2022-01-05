<template>
  <div>
    <DxPopup
      v-model:visible="popupVisible"
      :drag-enabled="false"
      :close-on-outside-click="false"
      :show-close-button="false"
      :show-title="true"
      :width="650"
      :height="580"
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
            <div
              class="col"
              v-if="
                messageType === messageTypeEnum.PRIVATE &&
                  !isUserSelectorReadOnly
              "
            >
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
            <div
              class="col"
              v-if="
                messageType === messageTypeEnum.OTHER_TEAM &&
                  !isUserSelectorReadOnly
              "
            >
              <label for="toTeamIdSelectBox" class="form-label"
                >Odbiorca (zarząd)</label
              >
              <DxSelectBox
                :dataSource="teams"
                value-expr="id"
                display-expr="name"
                v-model="ToTeamId"
                id="toTeamIdSelectBox"
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
              <DxHtmlEditor height="350" id="contentEditor" v-model="Content">
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
      teams: [],
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
      getTeamsToLookup: "jobAdvertisements/getTeamsToLookup",
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
      if (this.userIdFromProfile) {
        return "Odpowiedz na wiadomość";
      }
      switch (this.messageType) {
        case messageTypeEnum.PRIVATE:
          return "Wyślij wiadomość prywatną";
        case messageTypeEnum.TEAM:
          return "Wyślij wiadomość do swojej drużyny";
        case messageTypeEnum.OTHER_TEAM:
          return "Wyślij wiadomość do zarządu innej drużyny";
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
    } else if (this.messageType === messageTypeEnum.OTHER_TEAM) {
      this.getTeamsToLookup().then((response) => {
        this.teams = response.data;
      });
    }
    if (this.userIdFromProfile) {
      this.isUserSelectorReadOnly = true;
      if (this.messageType === messageTypeEnum.PRIVATE) {
        this.ToUserId = this.userIdFromProfile;
      } else if (this.messageType === messageTypeEnum.OTHER_TEAM) {
        this.ToTeamId = this.userIdFromProfile;
      }
    }
  },
  beforeUnmount() {
    this.RESET_TEAM_MESSAGE_FORM();
  },
};
</script>
