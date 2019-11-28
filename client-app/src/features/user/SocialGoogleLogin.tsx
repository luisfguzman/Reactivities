import React from "react";
import GoogleLogin from "react-google-login";
import { Button, Icon } from "semantic-ui-react";
import { observer } from "mobx-react-lite";

interface IProps {
  googleCallback: (response: any) => void;
  loading: boolean;
}

const SocialGoogleLogin: React.FC<IProps> = ({googleCallback, loading}) => {
  return (
    <div>
      <GoogleLogin
        clientId="441849648183-c1dt5q816h0qef8a8p0q01cph77goj25.apps.googleusercontent.com"
        buttonText="Login"
        onSuccess={googleCallback}
        onFailure={googleCallback}
        cookiePolicy={'single_host_origin'}
        render={(renderProps: any) => {
          return (
            <Button
            //   loading={loading}
              onClick={renderProps.onClick}
              color="google plus"
              fluid
              type="button"
            >
              <Icon name="google" />
              Continue with Google
            </Button>
          );
        }}
      />
    </div>
  );
};
export default observer(SocialGoogleLogin);