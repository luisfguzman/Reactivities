import React from "react";
import { Segment, Icon } from "semantic-ui-react";
import GoogleMapReact, { Coords } from "google-map-react";

interface IProps {
  lat: number;
  lng: number;
}

const Marker = () => <Icon name="marker" size="big" color="red" />;

const ActivityDetailedMap: React.FC<IProps> = ({ lat, lng }) => {
  const center: Coords = { lat, lng };
  const zoom = 14;
  return (
    <Segment attached="bottom" style={{ padding: 0 }}>
      <div style={{ height: "300px", width: "100%" }}>
        <GoogleMapReact
          bootstrapURLKeys={{ key: "AIzaSyAOhrJ11q5kFeZPhOZeoX-tDZ3gDlJVX7k" }}
          defaultCenter={center}
          defaultZoom={zoom}
        >
        </GoogleMapReact>
      </div>
    </Segment>
  );
};

export default ActivityDetailedMap;
