/*global google*/
import React, { useState, useContext, useEffect } from "react";
import { Segment, Form, Button, Grid } from "semantic-ui-react";
import { ActivityFormValues } from "../../../app/models/activity";
import { v4 as uuid } from "uuid";
import { observer } from "mobx-react-lite";
import { RouteComponentProps } from "react-router";
import { Form as FinalForm, Field } from "react-final-form";
import TextInput from "../../../app/common/form/TextInput";
import TextAreaInput from "../../../app/common/form/TextAreaInput";
import SelectInput from "../../../app/common/form/SelectInput";
import DateInput from "../../../app/common/form/DateInput";
import PlaceInput from "../../../app/common/form/PlaceInput";
import { category } from "../../../app/common/options/categoryOptions";
import { combineDateAndTime } from "../../../app/common/util/util";
import { RootStoreContext } from "../../../app/stores/rootStore";
import {
  combineValidators,
  isRequired,
  composeValidators,
  hasLengthGreaterThan,
  isNumeric
} from "revalidate";
import { geocodeByAddress, getLatLng } from "react-places-autocomplete";
import PriceInput from "../../../app/common/form/PriceInput";

const validate = combineValidators({
  title: isRequired({ message: "The event title is required" }),
  category: isRequired("Category"),
  description: composeValidators(
    isRequired("Description"),
    hasLengthGreaterThan(4)({
      message: "Description needs to be at least 5 characters"
    })
  )(),
  city: isRequired("City"),
  venue: isRequired("Venue"),
  date: isRequired("Date"),
  time: isRequired("Time"),
  price: composeValidators(isRequired("Price"),isNumeric("Price"))()
});

interface DetailParams {
  id: string;
}

const ActivityForm: React.FC<RouteComponentProps<DetailParams>> = ({
  match,
  history
}) => {
  const rootStore = useContext(RootStoreContext);
  const {
    createActivity,
    editActivity,
    submitting,
    loadActivity
  } = rootStore.activityStore;

  const [activity, setActivity] = useState(new ActivityFormValues());
  const [loading, setLoading] = useState(false);
  const [cityLatLng, setCityLatLng] = useState({ lat: 0, lng: 0 });
  const [venueLatLng, setVenueLatLng] = useState({ lat: 0, lng: 0 });

  useEffect(() => {
    if (match.params.id) {
      setLoading(true);
      loadActivity(match.params.id)
        .then(activity => {
          setActivity(new ActivityFormValues(activity));
        })
        .finally(() => setLoading(false));
    }
  }, [loadActivity, match.params.id]);

  const handleCitySelect = (selectedCity: string) => {
    geocodeByAddress(selectedCity)
      .then(results => getLatLng(results[0]))
      .then(latlng => {
        setCityLatLng(latlng);
      });
  };

  const handleVenueSelect = (selectedVenue: string) => {
    geocodeByAddress(selectedVenue)
      .then(results => getLatLng(results[0]))
      .then(latlng => {
        setVenueLatLng(latlng);
      });
  };

  const handleFinalFormSubmit = (values: any) => {
    const dateAndTime = combineDateAndTime(values.date, values.time);
    const { date, time, ...activity } = values;
    activity.date = dateAndTime;
    activity.lat = venueLatLng.lat !== 0 ? venueLatLng.lat : activity.lat;
    activity.lng = venueLatLng.lng !== 0 ? venueLatLng.lng : activity.lng;
    activity.price = Number(activity.price);
    if (!activity.id) {
      let newActivity = {
        ...activity,
        id: uuid()
      };
      createActivity(newActivity);
    } else {
      editActivity(activity);
    }
  };

  return (
    <Grid>
      <Grid.Column width={10}>
        <Segment clearing>
          <FinalForm
            validate={validate}
            initialValues={activity}
            onSubmit={handleFinalFormSubmit}
            render={({ handleSubmit, invalid, pristine }) => (
              <Form onSubmit={handleSubmit} loading={loading}>
                <Field
                  name="title"
                  placeholder="Title"
                  value={activity.title}
                  component={TextInput}
                />
                <Field
                  name="description"
                  placeholder="Description"
                  rows={3}
                  value={activity.description}
                  component={TextAreaInput}
                />
                <Form.Group widths="equal">
                  <Field
                    component={SelectInput}
                    options={category}
                    name="category"
                    placeholder="Category"
                    value={activity.category}
                  />
                  <Field
                    name="price"
                    placeholder="Price"
                    value={activity.price ? activity.price.toFixed(2): undefined}
                    component={PriceInput}
                  />
                </Form.Group>
                <Form.Group widths="equal">
                  <Field
                    component={DateInput}
                    name="date"
                    date={true}
                    placeholder="Date"
                    value={activity.date}
                  />
                  <Field
                    component={DateInput}
                    name="time"
                    time={true}
                    placeholder="Time"
                    value={activity.time}
                  />
                </Form.Group>

                <Field
                  component={PlaceInput}
                  name="city"
                  placeholder="City"
                  options={{ types: ["(cities)"] }}
                  value={activity.city}
                  onSelect={handleCitySelect}
                />
                <Field
                  component={PlaceInput}
                  name="venue"
                  placeholder="Venue"
                  options={{
                    location: new google.maps.LatLng(cityLatLng),
                    radius: 1000,
                    types: ["establishment"]
                  }}
                  value={activity.venue}
                  onSelect={handleVenueSelect}
                />
                <Button
                  loading={submitting}
                  disabled={loading || invalid || pristine}
                  floated="right"
                  positive
                  type="submit"
                  content="Submit"
                />
                <Button
                  onClick={
                    activity.id
                      ? () => history.push(`/activities/${activity.id}`)
                      : () => history.push("/activities")
                  }
                  disabled={loading}
                  floated="right"
                  type="button"
                  content="Cancel"
                />
              </Form>
            )}
          />
        </Segment>
      </Grid.Column>
    </Grid>
  );
};

export default observer(ActivityForm);
