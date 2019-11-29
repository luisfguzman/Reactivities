import React from "react";
import { Menu, Label, Icon } from "semantic-ui-react";

export const MessageMenu = () => {
  return (
    <Menu vertical>
      <Menu.Item name="unread">
        <Label color="teal">1</Label>
        <Icon name="envelope" /> Unread
      </Menu.Item>

      <Menu.Item name="inbox">
        <Label>51</Label>
        <Icon name="envelope open" />
        Inbox
      </Menu.Item>

      <Menu.Item name="outbox">
        <Label>51</Label>
        <Icon name="send" />
        Outbox
      </Menu.Item>
    </Menu>
  );
};
