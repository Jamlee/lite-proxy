import * as React from 'react';
import { Nav, INavProps } from 'office-ui-fabric-react/lib/Nav';
import { LineChart, XAxis, Tooltip, CartesianGrid, Line } from 'recharts';


export class Body extends React.Component {

    constructor(props) {

        super(props);

        this.data = [
            { name: 'Page A', uv: 4000, pv: 2400, amt: 2400 },
            { name: 'Page B', uv: 3000, pv: 1398, amt: 2210 },
            { name: 'Page C', uv: 2000, pv: 9800, amt: 2290 },
            { name: 'Page D', uv: 2780, pv: 3908, amt: 2000 },
            { name: 'Page E', uv: 1890, pv: 4800, amt: 2181 },
            { name: 'Page F', uv: 2390, pv: 3800, amt: 2500 },
            { name: 'Page G', uv: 3490, pv: 4300, amt: 2100 },
        ];


        this._onClickHandler = this._onClickHandler.bind(this);
    }

    render() {

        return (
            <div className="ms-Grid">
                <div className="ms-Grid-col ms-sm0 ms-md1 ms-lg2"></div>
                <div className="ms-Grid-col ms-sm12 ms-md10 ms-lg8">
                    <div className="ms-Grid mt-1">
                        <div className="ms-Grid-col ms-sm0 ms-md2">
                            <Nav
                                groups={[
                                    {
                                        links: [
                                            {
                                                name: '服务监控',
                                                url: 'http://example.com',
                                                links: [
                                                    {
                                                        name: '代理流量',
                                                        url: 'http://msn.com',
                                                        key: 'key1'
                                                    },
                                                    {
                                                        name: '运行状况',
                                                        url: 'http://msn.com',
                                                        key: 'key2'
                                                    }
                                                ],
                                                isExpanded: true
                                            }
                                        ]
                                    }
                                ]}
                                expandedStateText={'expanded'}
                                collapsedStateText={'collapsed'}
                                selectedKey={'key3'}
                            />
                        </div>
                        <div className="ms-Grid-col ms-sm12 ms-md10">
                            <LineChart
                                width={400}
                                height={400}
                                data={this.data}
                                margin={{ top: 5, right: 20, left: 10, bottom: 5 }}>
                                <XAxis dataKey="name" />
                                <Tooltip />
                                <CartesianGrid stroke="#f5f5f5" />
                                <Line type="monotone" dataKey="uv" stroke="#ff7300" yAxisId={0} />
                                <Line type="monotone" dataKey="pv" stroke="#387908" yAxisId={1} />
                            </LineChart>
                        </div>
                    </div>
                </div>
                <div className="ms-Grid-col ms-sm0 ms-md1 ms-lg2"></div>
            </div>
        );
    }

    _onClickHandler(e) {
        alert('test');
        return false;
    }

    _onClickHandler2(e) {
        return false;
    }
}