<template>
  <div>
    <DxPopup
      v-model:visible="popupVisible"
      :drag-enabled="false"
      :close-on-outside-click="false"
      :show-close-button="false"
      :show-title="true"
      :width="400"
      :height="500"
      container=".dx-viewport"
      :title="message.message.subject"
      :shading="false"
      v-if="message"
    >
      <DxToolbarItem
        widget="dxButton"
        toolbar="bottom"
        location="before"
        :options="deleteButtonOptions"
        v-if="!isSentMessage"
      />
      <DxToolbarItem
        widget="dxButton"
        toolbar="bottom"
        location="after"
        :options="closeButtonOptions"
      />
      <div>
        <span v-html="message.message.content"></span>
      </div>
    </DxPopup>
  </div>
</template>
<script>
import { DxPopup, DxToolbarItem } from "devextreme-vue/popup";
import { mapActions, mapMutations } from "vuex";
import { custom } from "devextreme/ui/dialog";

export default {
  name: "MessageDetails",
  props: {
    message: {
      type: Object,
      required: true,
    },
    isSentMessage: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      popupVisible: true,
      deleteButtonOptions: {
        text: "Usuń",
        icon: "trash",
        onClick: () => {
          this.delete();
        },
        type: "danger",
      },
      closeButtonOptions: {
        text: "Zamknij",
        onClick: () => {
          this.popupVisible = false;
          this.$emit("closed");
        },
      },
    };
  },
  components: {
    DxPopup,
    DxToolbarItem,
  },
  methods: {
    ...mapActions({
      markMessageAsRead: "messages/markMessageAsRead",
      deleteMessage: "messages/deleteMessage",
    }),
    ...mapMutations({
      RESET_TEAM_MESSAGE_FORM: "messages/RESET_TEAM_MESSAGE_FORM",
    }),
    delete() {
      let dialog = custom({
        title: "Usuwanie",
        messageHtml:
          "Czy na pewno chcesz usunąć wiadomość o tytule " +
          this.message.message.subject +
          "?",
        buttons: [
          {
            text: "Tak",
            onClick: () => {
              return true;
            },
          },
          {
            text: "Nie",
            onClick: () => {
              return false;
            },
          },
        ],
      });
      dialog.show().then((dialogResult) => {
        if (dialogResult === true) {
          this.deleteMessage(this.message);
          this.popupVisible = false;
          this.$emit("closed");
        }
      });
    },
  },
  mounted() {
    this.popupVisible = true;
    if (!this.isSentMessage && this.message.isRead === false) {
      this.markMessageAsRead(this.message);
    }
  },
  beforeUnmount() {
    this.RESET_TEAM_MESSAGE_FORM();
  },
};
</script>
