import React from "react";
import { Grid } from "semantic-ui-react";
import { MessageMenu } from "./MessageMenu";

export const MessageDashboard = () => {
  return (
    <Grid>
      <Grid.Column width={4}><MessageMenu /></Grid.Column>
      <Grid.Column width={12}>There</Grid.Column>
    </Grid>
  );
};
