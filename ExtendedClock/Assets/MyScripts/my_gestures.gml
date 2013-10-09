<?xml version="1.0" encoding="UTF-8"?>
<GestureMarkupLanguage xmlns:gml="http://gestureworks.com/gml/version/1.0" mouse_simulator="true">

	<Gesture_set gesture_set_name="basic-gestures">
			
		<Gesture id="NDrag" type="drag">
			<comment>The 'n-drag' gesture can be activated by any number of touch points. When a touch down is recognized on a touch object the position
			of the touch point is tracked. This change in the position of the touch point is mapped directly to the position of the touch object.</comment>
			<match>
				<action>
					<initial>
						<cluster point_number="0" point_number_min="1" point_number_max="10"/>
					</initial>
				</action>
			</match>	
			<analysis>
				<algorithm class="kinemetric" type="continuous">
					<library module="drag"/>
					<returns>
						<property id="drag_dx" result="dx"/>
						<property id="drag_dy" result="dy"/>
					</returns>
				</algorithm>
			</analysis>	
			<processing>
				<inertial_filter>
					<property ref="drag_dx" active="true" friction="0.9"/>
					<property ref="drag_dy" active="true" friction="0.9"/>
				</inertial_filter>
				<delta_filter>
					<property ref="drag_dx" active="true" delta_min="0.05" delta_max="500"/>
					<property ref="drag_dy" active="true" delta_min="0.05" delta_max="500"/>
				</delta_filter>
			</processing>
			<mapping>
				<update dispatch_type="continuous">
					<gesture_event type="drag">
						<property ref="drag_dx" target="x"/>
						<property ref="drag_dy" target="y"/>
					</gesture_event>
				</update>
			</mapping>
		</Gesture>
		
		<Gesture id="ThreeFingerTilt" type="tilt">
		  <match>
			<action>
			  <initial>
				<cluster point_number="3" point_number_min="3" point_number_max="3" separation_min="0.01"/>
			  </initial>
			</action>
		  </match>
		  <analysis>
			<algorithm class="kinemetric" type="continuous">
			  <library module="tilt"/>
			  <returns>
				<property id="tilt_dx" result="dsx"/>
				<property id="tilt_dy" result="dsy"/>
			  </returns>
			</algorithm>
		  </analysis>
		  <processing>
			<noise_filter>
			  <property ref="tilt_dx" noise_filter="false" percent="0"/>
			  <property ref="tilt_dy" noise_filter="false" percent="0"/>
			</noise_filter>
			<inertial_filter>
			  <property ref="tilt_dx" release_inertia="false" friction="0"/>
			  <property ref="tilt_dy" release_inertia="false" friction="0"/>
			</inertial_filter>
			<delta_filter>
			  <property ref="tilt_dx" delta_threshold="false" delta_min="0.0001" delta_max="1"/>
			  <property ref="tilt_dy" delta_threshold="false" delta_min="0.0001" delta_max="1"/>
			</delta_filter>
		  </processing>
		  <mapping>
			<update dispatch_type="continuous">
			  <gesture_event>
				<property ref="tilt_dx" target=""/>
				<property ref="tilt_dy" target=""/>
			  </gesture_event>
			</update>
		  </mapping>
		</Gesture>
			
		<Gesture id="NRotate" type="rotate">
			<match>
				<action>
					<initial>
						<cluster point_number="0" point_number_min="2" point_number_max="10"/>
					</initial>
				</action>
			</match>
			<analysis>
				<algorithm class="kinemetric" type="continuous">
					<library module="rotate"/>
					<returns>
						<property id="rotate_dtheta" result="dtheta"/>
					</returns>
				</algorithm>
			</analysis>	
			<processing>
				<inertial_filter>
					<property ref="rotate_dtheta" active="true" friction="0.9"/>
				</inertial_filter>
				<delta_filter>
					<property ref="rotate_dtheta" active="true" delta_min="0.01" delta_max="20"/>
				</delta_filter>
			</processing>
			<mapping>
				<update dispatch_type="continuous">
					<gesture_event type="rotate">
						<property ref="rotate_dtheta" target="rotate"/>
					</gesture_event>
				</update>
			</mapping>
		</Gesture>
		
		<Gesture id="NScale" type="scale">
			<match>
				<action>
					<initial>
						<cluster point_number="0" point_number_min="2" point_number_max="10"/>
					</initial>
				</action>
			</match>
			<analysis>
				<algorithm class="kinemetric" type="continuous">
					<library module="scale"/>
					<returns>
						<property id="scale_dsx" result="ds"/>
						<property id="scale_dsy" result="ds"/>
					</returns>
				</algorithm>
			</analysis>	
			<processing>
				<inertial_filter>
					<property ref="scale_dsx" active="true" friction="0.9"/>
					<property ref="scale_dsy" active="true" friction="0.9"/>
				</inertial_filter>
				<delta_filter>
					<property ref="scale_dsx" active="false" delta_min="0.0001" delta_max="1"/>
					<property ref="scale_dsy" active="false" delta_min="0.0001" delta_max="1"/>
				</delta_filter>
				<multiply_filter>
					<property ref="scale_dsx" active="true" func="linear" factor="0.0033"/>
					<property ref="scale_dsy" active="true" func="linear" factor="0.0033"/>
				</multiply_filter>
			</processing>
			<mapping>
				<update dispatch_type="continuous">
					<gesture_event type="scale">
						<property ref="scale_dsx" target="scaleX"/>
						<property ref="scale_dsy" target="scaleY"/>
					</gesture_event>
				</update>
			</mapping>
		</Gesture>	
		
	</Gesture_set>
	
	<Gesture_set gesture_set_name="temporal-gestures">
		<Gesture id="Tap" type="tap">
		  <match>
			<action>
			  <initial>
				<point event_duration_max="100" translation_max="10"/>
				<cluster point_number="1"  />
				<event touch_event="touchEnd"/>
			  </initial>
			</action>
		  </match>
		  <analysis>
			<algorithm class="temporalmetric" type="discrete">
			  <library module="tap"/>
			  <returns>
				<property id="tap_x" result="x"/>
				<property id="tap_y" result="y"/>
				<property id="tap_n" result="n"/>
			  </returns>
			</algorithm>
		  </analysis>
		  <mapping>
			<update dispatch_type="discrete" dispatch_mode="batch" dispatch_interval="50">
			  <gesture_event>
				<property ref="tap_x"/>
				<property ref="tap_y"/>
				<property ref="tap_n"/>
			  </gesture_event>
			</update>
		  </mapping>
		</Gesture>
	</Gesture_set>
	
</GestureMarkupLanguage>