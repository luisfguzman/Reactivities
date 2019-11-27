import React from "react";
import FacebookLogin from "react-facebook-login/dist/facebook-login-render-props";
import { Button, Icon } from "semantic-ui-react";
import { observer } from "mobx-react-lite";

interface IProps {
  fbCallback: (response: any) => void;
  loading: boolean;
}

const SocialFBLogin: React.FC<IProps> = ({fbCallback, loading}) => {
  return (
    <div>
      <FacebookLogin
        appId="1862517717225458"
        fields="name,email,picture"
        callback={fbCallback}
        render={(renderProps: any) => {
          return (
            <Button
              loading={loading}
              onClick={renderProps.onClick}
              color="facebook"
              fluid
              type="button"
            >
              <Icon name="facebook" />
              Continue with Facebook
            </Button>
          );
        }}
      />
    </div>
  );
};
export default observer(SocialFBLogin);