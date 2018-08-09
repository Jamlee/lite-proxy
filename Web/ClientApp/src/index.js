import React from "react";
import { render } from "react-dom";
import { Header, Body } from "./app";
import { Fabric } from 'office-ui-fabric-react/lib/Fabric';
import "office-ui-fabric-react/dist/sass/Fabric.scss";
import "./style/app.less";



class App extends React.Component {
    render() {
        return <div>
            <Fabric>
                <Header />
                <Body />
            </Fabric>
        </div>;
    }
}

render(<App />, document.getElementById("app"));