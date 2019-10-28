import React from "react";
import PlacesAutocomplete from "react-places-autocomplete";
import { FieldRenderProps } from "react-final-form";
import { FormFieldProps, Form, Label, Segment, List } from "semantic-ui-react";

interface IProps
  extends FieldRenderProps<string, HTMLInputElement>,
    FormFieldProps {}

const PlaceInput: React.FC<IProps> = ({
  input: { value, onChange, onBlur },
  width,
  options,
  placeholder,
  onSelect,
  meta: { touched, error }
}) => {
  return (
    <PlacesAutocomplete
      value={value}
      onChange={onChange}
      searchOptions={options}
      onSelect={onSelect}
    >
      {({ getInputProps, suggestions, getSuggestionItemProps, loading }) => (
        <Form.Field error={touched && !!error}>
          <input
            placeholder={placeholder}
            {...getInputProps({ placeholder, onBlur })}
          />
          {touched && error && (
            <Label basic color="red">
              {error}
            </Label>
          )}
          {suggestions.length > 0 && (
            <Segment
              style={{
                marginTop: 0,
                position: "absolute",
                zIndex: 1000,
                width: "100%"
              }}
            >
              {loading && <div>Loading...</div>}
              <List selection>
                {suggestions.map(suggestion => (
                  <List.Item {...getSuggestionItemProps(suggestion)}>
                    <List.Header>
                      {suggestion.formattedSuggestion.mainText}
                    </List.Header>
                    <List.Description>
                      {suggestion.formattedSuggestion.secondaryText}
                    </List.Description>
                  </List.Item>
                ))}
              </List>
            </Segment>
          )}
        </Form.Field>
      )}
    </PlacesAutocomplete>
  );
};

export default PlaceInput;
