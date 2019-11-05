import React, { useState } from "react";
import { Segment, Grid, Icon, Button } from "semantic-ui-react";
import { IActivity } from "../../../app/models/activity";
import { observer } from "mobx-react-lite";
import { format } from "date-fns";
import ActivityDetailedMap from "./ActivityDetailedMap";

const ActivityDetailedInfo: React.FC<{ activity: IActivity }> = ({
  activity
}) => {
  const [showMap, setShowMap] = useState(false);
  return (
    <Segment.Group>
      <Segment attached="top">
        <Grid>
          <Grid.Column width={1}>
            <Icon size="large" color="teal" name="info" />
          </Grid.Column>
          <Grid.Column width={15}>
            <p>{activity.description}</p>
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
              {format(activity.date, "eeee do MMMM")} at{" "}
              {format(activity.date, "h:mm a")}
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
              {activity.price > 0 ? activity.price.toFixed(2) : "Free"}
            </span>
          </Grid.Column>
        </Grid>
      </Segment>
      <Segment attached>
        <Grid verticalAlign="middle">
          <Grid.Column width={1}>
            <Icon name="marker" size="large" color="teal" />
          </Grid.Column>
          <Grid.Column width={11}>
            <span>
              {activity.venue}, {activity.city}
            </span>
          </Grid.Column>
          <Grid.Column width={4}>
            <Button
              onClick={() => setShowMap(!showMap)}
              color="teal"
              size="tiny"
              content={showMap ? "Hide Map" : "Show Map"}
            />
          </Grid.Column>
        </Grid>
      </Segment>
      {showMap && <ActivityDetailedMap lat={activity.lat} lng={activity.lng} />}
    </Segment.Group>
  );
};

export default observer(ActivityDetailedInfo);
