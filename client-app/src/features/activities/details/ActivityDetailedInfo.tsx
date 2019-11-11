import React, { useState, useContext } from "react";
import { Segment, Grid, Icon, Button, Form } from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import { format } from "date-fns";
import ActivityDetailedMap from "./ActivityDetailedMap";
import { Form as FinalForm } from "react-final-form";
import { RootStoreContext } from "../../../app/stores/rootStore";

const ActivityDetailedInfo = () => {
  const [showMap, setShowMap] = useState(false);
  const rootStore = useContext(RootStoreContext);
  const {
    sendLike,
    totalLikes,
    userLiked,
    activity
  } = rootStore.activityStore;

  return (
    <Segment.Group>
      <Segment attached="top">
        <Grid>
          <Grid.Column width={1}>
            <Icon size="large" color="teal" name="info" />
          </Grid.Column>
          <Grid.Column width={12}>
            <p>{activity!.description}</p>
          </Grid.Column>
          <Grid.Column width={3}>
          <FinalForm
            onSubmit={sendLike}
            render={({ handleSubmit, submitting, form }) => (
              <Form onSubmit={() => handleSubmit()!.then(() => form.reset())}>
                <Button
                  loading={submitting}
                  content={totalLikes}
                  labelPosition="left"
                  icon={userLiked ? "thumbs up" : "thumbs up outline"}
                  color="teal"
                />
              </Form>
            )}
          />
          </Grid.Column>
        </Grid>
      </Segment>
      <Segment attached>
        <Grid verticalAlign="middle">
          <Grid.Column width={1}>
            <Icon name="calendar" size="large" color="teal" />
          </Grid.Column>
          <Grid.Column width={15}>
            <span>
              {format(activity!.date, "eeee do MMMM")} at{" "}
              {format(activity!.date, "h:mm a")}
            </span>
          </Grid.Column>
        </Grid>
      </Segment>
      <Segment attached>
        <Grid verticalAlign="middle">
          <Grid.Column width={1}>
            <Icon name="dollar" size="large" color="teal" />
          </Grid.Column>
          <Grid.Column width={15}>
            <span>
              {activity!.price > 0 ? activity!.price.toFixed(2) : "Free"}
            </span>
          </Grid.Column>
        </Grid>
      </Segment>
      <Segment attached>
        <Grid verticalAlign="middle">
          <Grid.Column width={1}>
            <Icon name="marker" size="large" color="teal" />
          </Grid.Column>
          <Grid.Column width={12}>
            <span>
              {activity!.venue}, {activity!.city}
            </span>
          </Grid.Column>
          <Grid.Column width={3}>
            <Button
              onClick={() => setShowMap(!showMap)}
              color="teal"
              size="tiny"
              content={showMap ? "Hide Map" : "Show Map"}
            />
          </Grid.Column>
        </Grid>
      </Segment>
      {showMap && <ActivityDetailedMap lat={activity!.lat} lng={activity!.lng} />}
    </Segment.Group>
  );
};

export default observer(ActivityDetailedInfo);
