import React from "react";
import { FieldRenderProps } from "react-final-form";
import { FormFieldProps, Form, Label, Input } from "semantic-ui-react";

interface IProps
  extends FieldRenderProps<string, HTMLInputElement>,
    FormFieldProps {}

const PriceInput: React.FC<IProps> = ({
  input,
  width,
  type,
  placeholder,
  meta: { touched, error }
}) => {
  return (
    <Form.Field error={touched && !!error} type={type} width={width}>
      <Input labelPosition="right" type="text" placeholder="Amount">
        <Label basic>$</Label>
        <input {...input} placeholder={placeholder} />
        {touched && error && (
          <Label basic color="red">
            {error}
          </Label>
        )}
        <Label>.00</Label>
      </Input>
    </Form.Field>
  );
};

export default PriceInput;
