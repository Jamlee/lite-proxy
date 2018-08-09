import * as React from 'react';
import "./style.less";


export class Header extends React.Component {

    constructor(props) {
        super(props);
        this.state = { date: new Date() };
    }

    render() {
        return <header>
            <div className="padding"></div>
            <div className="navbar">
                <div className="ms-Grid">
                    <div className="ms-Grid-row">
                        <div className="ms-Grid-col ms-sm0 ms-md1 ms-lg2"></div>
                        <div className="ms-Grid-col ms-sm12 ms-md10 ms-lg8">
                            <span className="title">
                                LiteSocks
                        </span>
                            <ul className="menu">
                                <li>控制面板</li>
                                <li>系统设置</li>
                                <li></li>
                            </ul>
                        </div>
                        <div className="ms-Grid-col ms-sm0 ms-md1 ms-lg2"></div>
                    </div>
                </div>
            </div>
        </header>;
    }
}